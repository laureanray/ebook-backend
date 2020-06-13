using System.Collections.Generic;

namespace ebook_backend.Models
{
    public class Chapter
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }
     
        public string ChapterTitle { get; set; }
        public List<Topic> Topics { get; set; }
        public Exam Exam { get; set; }
    }
}