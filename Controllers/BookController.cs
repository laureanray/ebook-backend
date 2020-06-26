using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ebook_backend.Data;
using ebook_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ebook_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Book>> AddBook([FromBody] Book book)
        {
            book.DateCreated = DateTime.Now;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<Book>> GetAllBooks()
        {
            return await _context.Books
                .Include(b => b.Accesses)
                .Include(b => b.Chapters)
                .ThenInclude(a => a.Topics)
                .Include(b => b.Chapters)
                .ThenInclude(a => a.Exam).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(long id)
        {
            var book = await _context.Books
                .Include(b => b.Chapters)
                .ThenInclude(a => a.Exam)
                .ThenInclude(a => a.ExamItems)
                .ThenInclude(a => a.Choices)
                .Include(b => b.Chapters)
                .ThenInclude(a => a.Topics)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (book == null) return NotFound();
            return book;
        }
         
        
        [HttpGet("accessible/{studentId}")]
        public async Task<List<Book>> GetAccessibleBook(long studentId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            var access =
                await _context.Accesses.FirstOrDefaultAsync(a => a.Course == student.Course && a.Year == student.Year);
            var books = await _context.Books
                .Include(b => b.Accesses)
                .Include(b => b.Chapters)
                .ThenInclude(b => b.Exam)
                .Include(b => b.Chapters)
                .ThenInclude(b => b.Topics)
                .Where(b => b.Accesses.Any(a => a.Course == access.Course && a.Year == access.Year))
                .ToListAsync();
            return books;
        }
        
        [HttpGet("get-assigned/{studentId}/{bookId}")]
        public async Task<ActionResult<Instructor>> GetAssignedInstructor(long studentId, long bookId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null) return BadRequest("Student null");
 
            var ass = await _context.Assignments.Include(a => a.Book).Where(i => i.InstructorId == 1).ToListAsync();
            // foreach (var a in ass)
            // {
            //     Console.WriteLine(bookId + "==" +  a.Book.Id);
            //     Console.WriteLine(student.Course + "==" +  a.Course);
            //     Console.WriteLine(student.Year + "==" +  a.Year);
            //     Console.WriteLine(student.Section + "==" +  a.Section);
            // }
            var assignment = await _context.Assignments
                .Include(b => b.Book)
                .FirstOrDefaultAsync(a => a.Book.Id == bookId &&
                                          a.Course == student.Course &&
                                          a.Year == student.Year &&
                                          a.Section == student.Section);
            if (assignment == null) return BadRequest("Assignment null");
            var instructor = await _context.Instructors.Include(r => r.Assignments)
                .FirstOrDefaultAsync(r => r.Assignments.Contains(assignment));

            return instructor;
        }



        [HttpPost("topic/update")]
        public async Task<ActionResult<Topic>> UpdateTopic([FromBody] Topic topic)
        {
            var topicToUpdate = await _context.Topics.FirstOrDefaultAsync(t => t.Id == topic.Id);
            if (topicToUpdate == null) return NotFound();
            topicToUpdate.HtmlContent = topic.HtmlContent;
            topicToUpdate.LastUpdated = DateTime.Now;
            _context.Entry(topicToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return topicToUpdate;
        }

        [HttpPost("topic/add")]
        public async Task<ActionResult<Topic>> AddTopic([FromBody] Topic topic)
        {
            topic.LastUpdated = DateTime.Now;
            topic.HtmlContent = "Type content here.";
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
            return topic;
        }


        [HttpPost("update-cover")]
        public async Task<ActionResult<Book>> UpdateBookCover([FromBody] Book bookUpdate)
        {    
            var book = await _context.Books
                .Include(b => b.Chapters)
                .ThenInclude(a => a.Exam)
                .ThenInclude(a => a.ExamItems)
                .ThenInclude(a => a.Choices)
                .Include(b => b.Chapters)
                .ThenInclude(a => a.Topics)
                .FirstOrDefaultAsync(a => a.Id == bookUpdate.Id);
            if (book == null) return NotFound();
            book.BookCoverURL = bookUpdate.BookCoverURL;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }
        
        
        [HttpPost("update-details")]
        public async Task<ActionResult<Book>> UpdateDetails([FromBody] Book bookUpdate)
        {    
            var book = await _context.Books
                .Include(b => b.Chapters)
                .ThenInclude(a => a.Exam)
                .ThenInclude(a => a.ExamItems)
                .ThenInclude(a => a.Choices)
                .Include(b => b.Chapters)
                .ThenInclude(a => a.Topics)
                .FirstOrDefaultAsync(a => a.Id == bookUpdate.Id);
            if (book == null) return NotFound();
            book.BookTitle = bookUpdate.BookTitle;
            book.BookAuthor = bookUpdate.BookAuthor;
            book.BookDescription = bookUpdate.BookDescription;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }




        [HttpPost("chapter/add")]
        public async Task<ActionResult<Chapter>> AddChapter([FromBody] Chapter chapter)
        {
            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();
            return chapter;
        }

        [HttpPost("chapter/update")]
        public async Task<ActionResult<Chapter>> UpdateChapter([FromBody] Chapter chapter)
        {
            var chapterToUpdate = await _context.Chapters.FirstOrDefaultAsync(c => c.Id == chapter.Id);
            if (chapterToUpdate == null) return NotFound();
            chapterToUpdate.ChapterTitle = chapter.ChapterTitle;
            _context.Entry(chapterToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return chapter;
        }

        [HttpGet("topic/delete/{id}")]
        public async Task<ActionResult<Topic>> DeleteTopic(long id)
        {
            var topicToDelete = await _context.Topics.FirstOrDefaultAsync(t => t.Id == id);
            if (topicToDelete == null) return NotFound();
            _context.Topics.Remove(topicToDelete);
            await _context.SaveChangesAsync();
            return topicToDelete;
        }

        [HttpGet("chapter/delete/{id}")]
        public async Task<ActionResult<Chapter>> DeleteChapter(long id)
        {
            var chapterToDelete = await _context.Chapters.FirstOrDefaultAsync(c => c.Id == id);
            if (chapterToDelete == null) return NotFound();
            _context.Chapters.Remove(chapterToDelete);
            await _context.SaveChangesAsync();

            return chapterToDelete;
        }

        [HttpGet("delete-book/{id}")]
        public async Task<ActionResult<Book>> DeleteBook(long id)
        {
            var bookToDelete = await _context.Books.FirstOrDefaultAsync(c => c.Id == id);
            if (bookToDelete == null) return NotFound();
            _context.Books.Remove(bookToDelete);
            await _context.SaveChangesAsync();
            return bookToDelete; 
        }
        
        [HttpGet("removeAccess/{bookId}/{accessId}")]
        public async Task<ActionResult<Book>> RemoveAccess(long bookId, long accessId)
        {
            var book = await _context.Books
                .Include(a => a.Chapters)
                    .ThenInclude(q => q.Topics)
                .Include(a => a.Accesses)
                .FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null) return NotFound();
            var accessToRemove = book.Accesses.SingleOrDefault(c => c.Id == accessId);
            if (accessToRemove == null) return NotFound();
            book.Accesses.Remove(accessToRemove);

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return book;
        }

        // [HttpGet("makeAccessibleToAll/{bookId}")]
        // public async Task<ActionResult<Book>> MakeAccessibleToAll(long bookId)
        // {
        //     var book = await _context.Books
        //         .Include(a => a.Chapters)
        //         .ThenInclude(q => q.Topics)
        //         .Include(a => a.Accesses)
        //         .FirstOrDefaultAsync(b => b.Id == bookId);
        //
        //     if (book == null) return NotFound();
        //     
        //     book.AccessibleToAll = true;
        //     _context.Entry(book).State = EntityState.Modified;
        //     await _context.SaveChangesAsync();
        //
        //     return book;
        // }
        //
        // [HttpGet("makeNotAccessibleToAll/{bookId}")]
        // public async Task<ActionResult<Book>> MakeNotAccessibleToAll(long bookId)
        // {
        //     var book = await _context.Books
        //         .Include(a => a.Chapters)
        //         .ThenInclude(q => q.Topics)
        //         .Include(a => a.Accesses)
        //         .FirstOrDefaultAsync(b => b.Id == bookId);
        //
        //     if (book == null) return NotFound();
        //     
        //     book.AccessibleToAll = false;
        //     _context.Entry(book).State = EntityState.Modified;
        //     await _context.SaveChangesAsync();
        //
        //     return book;
        // }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        [HttpPost("upload-cover")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    // var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileName = GetTimestamp(DateTime.Now) + ".jpg";
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }


                    return Ok(new {dbPath});
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}