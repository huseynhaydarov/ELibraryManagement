using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELibraryManegement.Models
{
    public class Book
    {
        public int BookId { get; init; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public BookCategory Category { get; set; }
    }
}
