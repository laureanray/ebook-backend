using System;
using System.Collections.Generic;

namespace ebook_backend.Models
{
    public class Instructor
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string EmployeeNumber { get; set; }
        public string Honorifics { get; set; }
        public bool FirstLogin { get; set; }
        
        public List<Assignment> Assignments { get; set; } 
        
        public bool IsArchived { get; set; }
        
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}