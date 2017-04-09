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
                        YearPublished = reader["YearPublished"] as int?,
                        Genre = reader["Genre"].ToString(),
                        IsCheckedOut = reader["IsCheckedOut"] as bool?,
                        LastCheckedOutDate = reader["LastCheckedOutDate"] as int?,
                        DueBackDate = reader["DueBackDate"] as int?
                    });
                }

                connection.Close();
            }
            return rv;
        }

        [HttpGet]
        public IHttpActionResult GetBook(string title)
        {
            var rv = new Books();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = "SELECT title FROM LibraryTable";
                var cmd = new SqlCommand(query, connection);
                connection.Open();
                var reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    rv = (new Books
                    {
                        Title = reader["Title"].ToString()
                    });
                }
            }
            return Ok(Books.FirstOrDefault(f => String.Compare(title, f.Title) == 0));
        }

        [HttpPut]
        public IHttpActionResult CreateBook([FromBody] Books book)
        {
            //update a book
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = @"INSERT INTO[dbo].[LibraryTable] ([Title], [Author], [YearPublished], [Genre], [IsCheckedOut], [LastCheckedOutDate], [DueBackDate]) 
                                OUTPUT INSERTED.Id
                                VALUES (@Title, @Author, @YearPublished, @Genre, @IsCheckedOut, @LastCheckedOutDate, @DueBackDate)";
                var cmd = new SqlCommand(query, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@YearPublished", book.YearPublished);
                cmd.Parameters.AddWithValue("@Genre", book.Genre);
                cmd.Parameters.AddWithValue("@IsCheckedOut", book.IsCheckedOut);
                cmd.Parameters.AddWithValue("@LastCheckedOutDate", book.LastCheckedOutDate);
                cmd.Parameters.AddWithValue("@DueBackDate", book.DueBackDate);
                var newID = cmd.ExecuteScalar();
                book.ID = (int)newID;
                connection.Close();
            }
            return Ok(book);
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

            using (var connection = new SqlConnection(ConnectionString))
            {
                var text = @"INSERT INTO LibraryTable (Title, Author, Genre)" +
                            "Values (@Title, @Author, @Genre)";

                cmd.Parameters.AddWithValue("@Title", found);
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
