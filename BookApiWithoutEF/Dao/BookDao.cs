using BookApiWithoutEF.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Dao
{
    public interface BookDao
    {
        IList<Book> Search(SqlConnection con);
        string Insert(Book book, SqlConnection con, SqlTransaction trx);
        string Delete(int id, SqlConnection con, SqlTransaction trx);
        Book SearchById(int id, SqlConnection con);
        string Update(Book book, SqlConnection con, SqlTransaction trx);
    }
}
