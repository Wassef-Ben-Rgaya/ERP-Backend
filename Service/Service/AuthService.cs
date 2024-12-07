using AutoMapper;
using Core.Entities;
using DAL.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Service.DTO;
using Service.IService;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryAsync<Personnel> _personnelRepository;
        private readonly IRepositoryAsync<Département> _departementRepository;
        private readonly IRepositoryAsync<Token> _tokenRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public AuthService(IRepositoryAsync<Personnel> personnelRepository, IRepositoryAsync<Token> tokenRepository, IRepositoryAsync<Département> departementRepository, IMapper mapper, ILogger logger, IConfiguration configuration)
        {
            _personnelRepository = personnelRepository ?? throw new ArgumentNullException(nameof(personnelRepository));
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
            _departementRepository = departementRepository ?? throw new ArgumentNullException(nameof(departementRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<(bool, string)> Register(RegisterDto registerDto)
        {
            var departement = await _departementRepository.GetFirstOrDefault(d => d.Departementid == registerDto.DepartementId);
            if (departement == null)
            {
                _logger.Warning("Department does not exist.");
                return (false, "Department does not exist.");
            }

            var userByEmail = await _personnelRepository.GetFirstOrDefault(p => p.Email == registerDto.Email);
            if (userByEmail != null)
            {
                _logger.Warning("Email already exists.");
                return (false, "Email already exists.");
            }

            var userByMatricule = await _personnelRepository.GetFirstOrDefault(p => p.Matricule == registerDto.Matricule);
            if (userByMatricule != null)
            {
                _logger.Warning("Matricule already exists.");
                return (false, "Matricule already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Mdp);
            var personnel = new Personnel
            {
                Nom = registerDto.Nom,
                Prenom = registerDto.Prenom,
                Datenaissance = DateOnly.FromDateTime(registerDto.DateNaissance),
                Adresse = registerDto.Adresse,
                Email = registerDto.Email,
                Mdp = hashedPassword,
                Poste = registerDto.Poste,
                Dateembauche = DateOnly.FromDateTime(registerDto.DateEmbauche),
                Statutfamiliale = registerDto.StatutFamiliale,
                Typecontrat = registerDto.TypeContrat,
                Numerotelephone = registerDto.NumeroTelephone,
                Departementid = registerDto.DepartementId,
                Matricule = registerDto.Matricule
            };

            await _personnelRepository.Add(personnel);
            _logger.Information($"User {personnel.Email} registered successfully.");
            return (true, "User registered successfully.");
        }

        public async Task<AuthenticationDto> Login(LoginDto loginDto)
        {
            var user = await _personnelRepository.GetFirstOrDefault(p => p.Matricule == loginDto.Matricule);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Mdp, user.Mdp))
            {
                _logger.Warning("Invalid login attempt.");
                return null;
            }

            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            var tokenEntity = new Token
            {
                Matricule = user.Matricule,
                Tokenvalue = refreshToken,
                Expiration = DateTime.UtcNow.AddDays(7) // Refresh token validity
            };

            await _tokenRepository.Add(tokenEntity);

            _logger.Information($"User {user.Matricule} logged in successfully.");

            return new AuthenticationDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1),
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthenticationDto> RefreshToken(RefreshTokenRequestDto refreshTokenRequest)
        {
            var tokenEntity = await _tokenRepository.GetFirstOrDefault(t => t.Tokenvalue == refreshTokenRequest.RefreshToken);
            if (tokenEntity == null || tokenEntity.Expiration < DateTime.UtcNow)
            {
                _logger.Warning("Invalid or expired refresh token.");
                return null;
            }

            var user = await _personnelRepository.GetById(tokenEntity.Matricule);
            if (user == null)
            {
                _logger.Warning("User not found for the refresh token.");
                return null;
            }

            var newToken = await GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            tokenEntity.Tokenvalue = newRefreshToken;
            tokenEntity.Expiration = DateTime.UtcNow.AddDays(7);

            await _tokenRepository.Update(tokenEntity);

            return new AuthenticationDto
            {
                Token = newToken,
                Expiration = DateTime.UtcNow.AddHours(1),
                RefreshToken = newRefreshToken
            };
        }

        public async Task<PersonnelDto> GetCurrentUserDetails(ClaimsPrincipal userClaims)
        {
            var matricule = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (matricule == null)
            {
                _logger.Warning("User is not authenticated.");
                return null;
            }

            var user = await _personnelRepository.GetById(Convert.ToInt32(matricule));
            if (user == null)
            {
                _logger.Warning($"User with ID {matricule} not found.");
                return null;
            }

            return _mapper.Map<PersonnelDto>(user);
        }

        public async Task<string> GenerateJwtToken(Personnel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Matricule.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Poste)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:TokenExpiryInMinutes"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
