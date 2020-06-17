namespace ebook_backend.Models
{
    public class Access
    {
        public long Id { get; set; }
        public string Course { get; set; }
        public string Year { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }
    }
}