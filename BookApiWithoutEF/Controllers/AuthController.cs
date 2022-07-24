using BookApiWithoutEF.Model;
using BookApiWithoutEF.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _authRepository;

        public AuthController(AuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost]
        public ActionResult<LoginResponse> Login(Login request)
        {
            LoginResponse loginResponse = _authRepository.Login(request);
            return loginResponse;
        }

        [HttpPost("refresh-token")]
        public ActionResult<LoginResponse> RefreshToken(RefreshToken request)
        {
            LoginResponse loginResponse = _authRepository.RefreshToken(request);
            return loginResponse;
        }
    }
}
