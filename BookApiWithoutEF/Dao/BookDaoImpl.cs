using BookApiWithoutEF.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Dao
{
    public class BookDaoImpl : BookDao
    {
        public IList<Book> Search(SqlConnection con)
        {
            SqlDataReader dataReader = null;
            Book book = null;
            IList<Book> books = new List<Book>();

            SqlCommand cmd = new SqlCommand("SP_SEARCH", con);
            cmd.CommandType = CommandType.StoredProcedure;

            dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                book = new Book();
                book.BookId = dataReader.GetInt32(0);
                book.Title = dataReader.GetString(1);
                book.Author = dataReader.GetString(2);
                book.Price = dataReader.GetInt32(3);

                books.Add(book);
            }

            return books;
        }

        public Book SearchById(int id, SqlConnection con)
        {
            SqlDataReader dataReader = null;
            Book book = null;

            SqlCommand cmd = new SqlCommand("SP_SEARCH_BY_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bookId", id);

            dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                book = new Book();
                book.BookId = dataReader.GetInt32(0);
                book.Title = dataReader.GetString(1);
                book.Author = dataReader.GetString(2);
                book.Price = dataReader.GetInt32(3);
            }

            return book;
        }

        public string Insert(Book book, SqlConnection con, SqlTransaction trx)
        {
            SqlCommand cmd = new SqlCommand("SP_INSERT", con, trx);
            cmd.CommandType = CommandType.StoredProcedure;

            var outputMessage = new SqlParameter("@OutputMessage", SqlDbType.VarChar, 2000);
            outputMessage.Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Price", book.Price);
            cmd.Parameters.Add(outputMessage);

            cmd.ExecuteNonQuery();

            string message = outputMessage.Value.ToString();

            return message;
        }

        public string Delete(int id, SqlConnection con, SqlTransaction trx)
        {
            SqlCommand cmd = new SqlCommand("SP_DELETE", con, trx);
            cmd.CommandType = CommandType.StoredProcedure;

            var outputMessage = new SqlParameter("@OutputMessage", SqlDbType.VarChar, 2000);
            outputMessage.Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@bookId", id);
            cmd.Parameters.Add(outputMessage);

            cmd.ExecuteNonQuery();

            string message = outputMessage.Value.ToString();

            return message;
        }
        
        public string Update(Book book, SqlConnection con, SqlTransaction trx)
        {
            SqlCommand cmd = new SqlCommand("SP_UPDATE", con, trx);
            cmd.CommandType = CommandType.StoredProcedure;

            var outputMessage = new SqlParameter("@OutputMessage", SqlDbType.VarChar, 2000);
            outputMessage.Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@bookid", book.BookId);
            cmd.Parameters.AddWithValue("@title", book.Title);
            cmd.Parameters.AddWithValue("@author", book.Author);
            cmd.Parameters.AddWithValue("@price", book.Price);
            cmd.Parameters.Add(outputMessage);

            cmd.ExecuteNonQuery();

            string message = outputMessage.Value.ToString();

            return message;
        }
    }
}
