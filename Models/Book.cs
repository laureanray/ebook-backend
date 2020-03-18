using System;

namespace ebook_backend.Models
{
    public class Book
    {
        public long Id { get; set; }
        public string BookTitle { get; set; }
        public string BookDescription { get; set; }
        public string BookAuthor { get; set; }
        
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}