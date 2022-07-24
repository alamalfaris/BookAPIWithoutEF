using BookApiWithoutEF.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Dao
{
    public class AuthDaoImpl : AuthDao
    {
        public string Login(Login login, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SP_LOGIN", con);
            cmd.CommandType = CommandType.StoredProcedure;

            var outputMessage = new SqlParameter("@outputMessage", SqlDbType.VarChar, 2000);
            outputMessage.Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@username", login.Username);
            cmd.Parameters.AddWithValue("@password", login.Password);
            cmd.Parameters.Add(outputMessage);

            cmd.ExecuteNonQuery();

            string message = outputMessage.Value.ToString();

            return message;
        }

        public string GetRole(string username, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SP_GET_ROLE", con);
            cmd.CommandType = CommandType.StoredProcedure;            

            var roleName = new SqlParameter("@rolename", SqlDbType.VarChar, 2000);
            roleName.Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.Add(roleName);

            cmd.ExecuteNonQuery();

            string role = roleName.Value.ToString();

            return role;
        }

        public string GenerateRefreshToken(string username, RefreshToken refreshToken, SqlConnection con, SqlTransaction trx)
        {
            SqlCommand cmd = new SqlCommand("SP_GENERATE_REFRESH_TOKEN", con, trx);
            cmd.CommandType = CommandType.StoredProcedure;

            var outputMessage = new SqlParameter("@outputMessage", SqlDbType.VarChar, 2000);
            outputMessage.Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@refreshToken", refreshToken.Token);
            cmd.Parameters.AddWithValue("@expired", refreshToken.Expired);
            cmd.Parameters.Add(outputMessage);

            cmd.ExecuteNonQuery();

            string message = outputMessage.Value.ToString();

            return message;

        }

        public string ValidateRefreshToken(RefreshToken refreshToken, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SP_CHECK_REFRESH_TOKEN", con);
            cmd.CommandType = CommandType.StoredProcedure;

            var outputMessage = new SqlParameter("@outputMessage", SqlDbType.VarChar, 2000);
            outputMessage.Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@username", refreshToken.Username);
            cmd.Parameters.AddWithValue("@refreshToken", refreshToken.Token);
            cmd.Parameters.Add(outputMessage);

            cmd.ExecuteNonQuery();

            string message = outputMessage.Value.ToString();

            return message;
        }
    }
}
