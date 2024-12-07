using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
