using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Model
{
    public class RefreshToken
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime Expired { get; set; }
    }
}
