using System;

namespace ebook_backend.Models
{
    public class Topic
    {
        public long Id { get; set; }
        public string TopicTitle { get; set; }
        public string HtmlContent { get; set; }
        public long ChapterId { get; set; }
        public Chapter Chapter { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}