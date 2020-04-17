using System;
using System.Collections.Generic;

namespace ebook_backend.Models
{
    public class Course
    {
        public long Id { get; set; }
        public string CourseName { get; set; }
        // public List<short> Year { get; set; }
        public List<Year> Years { get; set; }
    }
}