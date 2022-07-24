using BookApiWithoutEF.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Dao
{
    public interface AuthDao
    {
        string Login(Login login, SqlConnection con);
        string GetRole(string username, SqlConnection con);
        string GenerateRefreshToken(string username, RefreshToken refreshToken, SqlConnection con, SqlTransaction trx);
        string ValidateRefreshToken(RefreshToken refreshToken, SqlConnection con);
    }
}
