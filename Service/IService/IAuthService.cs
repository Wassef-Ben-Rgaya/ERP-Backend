using Service.DTO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IAuthService
    {
        Task<(bool, string)> Register(RegisterDto registerDto);
        Task<AuthenticationDto> Login(LoginDto loginDto);
        Task<AuthenticationDto> RefreshToken(RefreshTokenRequestDto refreshTokenRequest);
        Task<PersonnelDto> GetCurrentUserDetails(ClaimsPrincipal userClaims);
    }
}
