namespace ebook_backend.Models
{
    public class BookProgress
    {
        public long Id { get; set; }
        public Book Book { get; set; }
        public long BookId { get; set; }
        public string LatestProgress { get; set; }
        public Student Student { get; set; }
    }
}