using System;
using System.Collections.Generic;

namespace ebook_backend.Models
{
    public class Exam
    {
        public long Id { get; set; }
        public string Instructions { get; set; } 
        public List<ExamItem> ExamItems { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Chapter Chapter { get; set; }
        public long ChapterId { get; set; }
        public List<Grade> Grades { get; set; }
    }
}