using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.DTO;
using Service.IService;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    [EnableCors("CORSPolicy")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var (success, message) = await _authService.Register(registerDto);
                if (success)
                {
                    return Ok(message);
                }
                return Conflict(new { message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration.");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var authResult = await _authService.Login(loginDto);
                if (authResult == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                SetTokenCookie(authResult.RefreshToken);

                return Ok(authResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Refresh Token
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto refreshTokenRequest)
        {
            try
            {
                var authResult = await _authService.RefreshToken(refreshTokenRequest);
                if (authResult == null)
                {
                    return Unauthorized("Invalid refresh token.");
                }

                SetTokenCookie(authResult.RefreshToken);

                return Ok(authResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during token refresh.");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Get Current User Details
        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserDetails()
        {
            try
            {
                var user = await _authService.GetCurrentUserDetails(User);
                if (user == null)
                {
                    return Unauthorized("User not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user details.");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private void SetTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
