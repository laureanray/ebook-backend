using System;
using System.Collections.Generic;

namespace ebook_backend.Models
{
    public class ExamItem
    {
        public long Id { get; set; }
        public ExamType ExamType { get; set; }
        public string Answer { get; set; }
        public List<Choice> Choices { get; set; } 
        public long ExamId { get; set; }
        public Exam Exam { get; set; }
    }
}      