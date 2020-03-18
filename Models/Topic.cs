namespace ebook_backend.Models
{
    public class Topic
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public long ChapterId { get; set; }
        public Chapter Chapter { get; set; }
    }
}