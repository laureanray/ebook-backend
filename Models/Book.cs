using System;
using System.Collections.Generic;

namespace ebook_backend.Models
{
    public class Book
    {
        public long Id { get; set; }
        public string BookTitle { get; set; }
        public string BookDescription { get; set; }
        public string BookAuthor { get; set; }
        public bool AccessibleToAll { get; set; }
        public List<Course> Courses { get; set; }
        public List<Chapter> Chapters { get; set; }
        public string BookCoverURL { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}