using System.Runtime.CompilerServices;

namespace ebook_backend.Models
{
    public class CompileTask
    {
        public string Source { get; set; }
        public string Output { get; set; }
        public string CompileOutput { get; set; }
        public bool IsError { get; set; }
        public string RuntimeError { get; set; }
        public string CompileError { get; set; }
    }
}