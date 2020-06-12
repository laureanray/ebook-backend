using System;
using System.Collections.Generic;

namespace ebook_backend.Models
{
    public class Student
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string StudentNumber { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string YearLevel { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public bool IsArchived { get; set; }
        public bool FirstLogin { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public virtual  IEnumerable<BookProgress> BookProgresses { get; set; }
    }
}