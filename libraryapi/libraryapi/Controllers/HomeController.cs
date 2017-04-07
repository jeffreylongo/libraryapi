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

        [HttpGet]
        public IEnumerable<Books> GetAllBooks()
        {
            return Books;
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

        public List<Books> Library { get; set; } = new List<Books>();
        const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=libraryapi;Trusted_Connection=True;";
        static private sqlDataReader ExecuteQuery(string query)
        {
            sqlDataReader rv;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand(query, connection);
                connection.Open();
                rv = cmd.ExecuteReader();
                connection.Close();
            }
            return rv;
        }

        /*public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }*/
    }
}
