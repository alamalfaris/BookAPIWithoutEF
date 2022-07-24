using BookApiWithoutEF.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiWithoutEF.Repository
{
    public interface BookRepository
    {
        Response Search();
        Response Insert(Book book);
        Response Delete(int id);
        Response SearchById(int id);
        Response Update(Book book);
    }
}
