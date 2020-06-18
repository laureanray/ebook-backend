namespace ebook_backend.Models
{
    public class Assignment
    {
        public long Id { get; set; }

        public Book Book { get; set; }
        public long BookId { get; set; }
        public string Course { get; set; }
        public string Year { get; set; }
        public string Section { get; set; }
        public Instructor Instructor { get; set; }
        public long InstructorId { get; set; }
    }
}