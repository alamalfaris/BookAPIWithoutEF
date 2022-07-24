using BookApiWithoutEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Repository
{
    public interface AuthRepository
    {
        LoginResponse Login(Login login);
        LoginResponse RefreshToken(RefreshToken refreshToken);
    }
}
