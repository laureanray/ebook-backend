using System;

namespace ebook_backend.Models
{
    public class Grade
    {
        public long Id { get; set; }
        public Exam Exam { get; set; }
        public long ExamId { get; set; }
        public Student Student { get; set; }
        public long StudentId { get; set; }
        public long Score { get; set; }
        public DateTime DateCreated { get; set; }
    }
}