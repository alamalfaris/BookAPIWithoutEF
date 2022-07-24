using BookApiWithoutEF.Dao;
using BookApiWithoutEF.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Repository
{
    public class AuthRespositoryImpl : AuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly AuthDao _authDao;

        public AuthRespositoryImpl(IConfiguration configuration, AuthDao authDao)
        {
            _configuration = configuration;
            _authDao = authDao;
        }

        public LoginResponse Login(Login login)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            
            LoginResponse loginResponse = new LoginResponse();
            string message = null;
            string token = null;
            RefreshToken refreshToken = new RefreshToken();
            string role = null;

            try
            {
                con.Open();
                message = _authDao.Login(login, con);
            }
            catch (Exception error)
            {
                message = error.Message;
            }
            finally
            {
                con.Close();
            }

            if (message == "Login success")
            {
                con.Open();
                role = _authDao.GetRole(login.Username, con);
                con.Close();

                if (role != null || role != "")
                {
                    token = CreateToken(login.Username, role);
                    refreshToken = GenerateRefreshToken(login.Username);
                }                
            }

            loginResponse.Message = message;
            loginResponse.Username = login.Username;
            loginResponse.Role = role;
            loginResponse.Token = token;
            loginResponse.TokenExpired = DateTime.Now.AddDays(1);
            loginResponse.RefreshToken = refreshToken.Token;
            loginResponse.RefreshTokenExpired = refreshToken.Expired;

            return loginResponse;
        }

        public RefreshToken GenerateRefreshToken(string userName)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlTransaction trx = null;
            RefreshToken refreshToken = new RefreshToken();
            string message = null;

            byte[] num = new byte[RandomNumberGenerator.GetInt32(64)];
            refreshToken.Token = Convert.ToBase64String(num);
            refreshToken.Expired = DateTime.Now.AddDays(7);

            try
            {
                con.Open();
                trx = con.BeginTransaction();
                message = _authDao.GenerateRefreshToken(userName, refreshToken, con, trx);
                if (message == "FAILED")
                {
                    trx.Rollback();
                    refreshToken.Token = "FAILED";
                }
                else
                {
                    trx.Commit();
                }
            }   
            catch (Exception error)
            {
                trx.Rollback();
                message = error.Message;
                refreshToken.Token = message;
            }
            finally
            {
                con.Close();
            }

            return refreshToken;
        }

        private string CreateToken(string userName, string role)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public LoginResponse RefreshToken(RefreshToken refreshToken)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            string message = null;
            string role = null;
            string token = null;

            try
            {
                con.Open();
                message = _authDao.ValidateRefreshToken(refreshToken, con);
                if (message == "Valid")
                {
                    role = _authDao.GetRole(refreshToken.Username, con);
                    token = CreateToken(refreshToken.Username, role);
                }
            }
            catch (Exception error)
            {
                message = error.Message;
            }
            finally
            {
                con.Close();
            }

            LoginResponse loginResponse = new LoginResponse();
            loginResponse.Username = refreshToken.Username;
            loginResponse.Role = role;
            loginResponse.Message = message;
            loginResponse.Token = token;
            loginResponse.TokenExpired = DateTime.Now.AddDays(1);

            return loginResponse;
        }
    }
}
