using System;

namespace ebook_backend.Models
{
    public class Instructor
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}