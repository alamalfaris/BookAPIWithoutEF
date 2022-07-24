using BookApiWithoutEF.Dao;
using BookApiWithoutEF.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Repository
{
    public class BookRepositoryImpl : BookRepository
    {
        private readonly IConfiguration _configuration;
        private readonly BookDao _bookDao;

        public BookRepositoryImpl(IConfiguration configuration, BookDao bookDao)
        {
            _configuration = configuration;
            _bookDao = bookDao;
        }        

        public Response Search()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            IList<Book> books = new List<Book>();
            string errMesg = null;
            Response response = new Response();
            
            try
            {
                con.Open();
                books = _bookDao.Search(con);
            }
            catch (Exception error)
            {
                errMesg = error.Message;
            }
            finally
            {
                con.Close();
                response.Message = errMesg;
                response.Data = books;
            }

            return response;

        }

        public Response SearchById(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            Book book = null;
            string errMesg = null;
            Response response = new Response();

            try
            {
                con.Open();
                book = _bookDao.SearchById(id, con);
            }
            catch (Exception error)
            {
                errMesg = error.Message;
            }
            finally
            {
                con.Close();
            }

            response.Message = errMesg;
            response.Data = book;

            return response;
        }

        public Response Insert(Book book)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlTransaction trx = null;
            Response response = new Response();
            string message = null;
            string errMesg = null;

            try
            {
                con.Open();
                trx = con.BeginTransaction();
                message = _bookDao.Insert(book, con, trx);
                trx.Commit();
            }
            catch (Exception error)
            {
                trx.Rollback();
                errMesg = error.Message;
            }
            finally
            {                
                con.Close();
            }

            response.Message = errMesg;
            response.Data = message;

            return response;
        }

        public Response Delete(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlTransaction trx = null;
            Response response = new Response();
            string message = null;
            string errMesg = null;

            try
            {
                con.Open();
                trx = con.BeginTransaction();
                message = _bookDao.Delete(id, con, trx);
                trx.Commit();
            }
            catch (Exception error)
            {
                trx.Rollback();
                errMesg = error.Message;
            }
            finally
            {
                con.Close();
            }

            response.Message = errMesg;
            response.Data = message;

            return response;
        }

        public Response Update(Book book)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlTransaction trx = null;
            string message = null;
            string errMesg = null;
            Response response = new Response();

            try
            {
                con.Open();                
                trx = con.BeginTransaction();
                message = _bookDao.Update(book, con, trx);
                trx.Commit();
            }                
            catch (Exception error)
            {
                trx.Rollback();
                errMesg = error.Message;
            }
            finally
            {
                con.Close();
            }

            response.Message = errMesg;
            response.Data = message;

            return response;
        }
    }
}
