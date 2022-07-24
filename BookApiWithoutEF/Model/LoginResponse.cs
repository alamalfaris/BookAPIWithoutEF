using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Model
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpired { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpired { get; set; }
    }
}
