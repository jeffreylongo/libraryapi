using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace libraryapi.Models
{
    public class Books
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int? YearPublished { get; set; }
        public string Genre { get; set; }
        public bool? IsCheckedOut { get; set; }
        public int? LastCheckedOutDate { get; set; }
        public int? DueBackDate { get; set; }
    }
}