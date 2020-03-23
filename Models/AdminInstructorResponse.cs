namespace ebook_backend.Models
{
    public class AdminInstructorResponse
    {
        public string Type { get; set; }
        public Admin Admin { get; set; }
        public Instructor Instructor { get; set; }
    }
}