using libraryapi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace libraryapi.Controllers
{
    public class HomeController : ApiController
    {
        public static List<Books> Books { get; set; } = new List<Books>
        {
            new Books {Title = "The Gunslinger", Author = "Stephen King", YearPublished = 1982, Genre = "Horror", IsCheckedOut = false, LastCheckedOutDate = 0, DueBackDate = 0 }
        };

        const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=libraryapi;Trusted_Connection=True;";


        [HttpGet]
        public IEnumerable<Books> GetAllBooks()
        {
            var rv = new List<Books>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = "SELECT * FROM LibraryTable";
                var cmd = new SqlCommand(query, connection);
                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rv.Add(new Books
                    {
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                    });
                }

                connection.Close();
            }
            return rv;
        }

        [HttpGet]
        public IHttpActionResult GetBook(string title)
        {
            return Ok(Books.FirstOrDefault(f => String.Compare(title, f.Title) == 0));
        }

        [HttpPut]
        public Books PutBook(string title, string author)
        {
            var b = new Books { Title = title, Author = author };
            Books.Add(b);
            return b;
        }

        [HttpPost]
        public IHttpActionResult PostBook(Books updated)
        {
            var found = Books.FirstOrDefault(f => f.Id == updated.Id);
            if (found == null)
            {
                return NotFound();
            }
            else
            {
                found.Title = updated.Title;
                return Ok(found);
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteBook(Guid id)
        {
            Books = Books.Where(w => w.Id != id).ToList();
            return Ok();
        }


        

        /*static private int GetCountFromTable(string LibraryTable, string column = "Title")
        {
            var count = 0;
            var query = $"SELECT COUNT({Title}) FROM {LibraryTable}";
            var reader = ExecuteQuery(query);
            while (reader.Read())
            {
                count = (int)reader[0];
            }
            return count;
        }*/

        /*public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }*/
    }
}
