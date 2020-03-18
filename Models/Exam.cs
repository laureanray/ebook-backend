using System;
using System.Collections.Generic;

namespace ebook_backend.Models
{
    public class Exam
    {
        public long Id { get; set; }

        public List<ExamItem> ExamItems { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}