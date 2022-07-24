using BookApiWithoutEF.Model;
using BookApiWithoutEF.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        //private readonly IConfiguration _configuration;
        private readonly BookRepository _bookRepository;

        public BookController(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        public ActionResult<Response> Search()
        {
            Response response = _bookRepository.Search();            
            return Ok(response);
        }

        [Authorize(Roles = "user")]
        [HttpGet("{id}")]
        public ActionResult<Response> SearchById(int id)
        {
            Response response = _bookRepository.SearchById(id);
            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult<Response> Insert(Book request)
        {
            Response response = _bookRepository.Insert(request);
            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public ActionResult<Response> Delete(int id)
        {
            Response response = _bookRepository.Delete(id);
            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public ActionResult<Response> Update(Book request)
        {
            Response response = _bookRepository.Update(request);
            return Ok(response);
        }
    }
}
