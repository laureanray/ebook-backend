namespace ebook_backend.Models
{
    public class Choice
    {
        public long Id { get; set; }
        public long ExamItemId { get; set; }
        public ExamItem ExamItem { get; set; }
        public string ChoiceText { get; set; }
    }
}