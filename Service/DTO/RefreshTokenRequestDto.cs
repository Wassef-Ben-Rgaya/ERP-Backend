using System;

namespace Service.DTO
{
    public class RefreshTokenRequestDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
