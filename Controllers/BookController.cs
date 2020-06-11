using System;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<List<Book>> GetAllBooks()
        {
            return await _context.Books
                .Include(b => b.Chapters)
                .ThenInclude(a => a.Topics)
                .Include(b => b.Courses)
                .ThenInclude(a => a.Years).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(long id)
        {
            var book = await _context.Books
                .Include(b => b.Chapters)
                .ThenInclude(a => a.Topics)
                .Include(b => b.Courses)
                .ThenInclude(a => a.Years)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (book == null) return NotFound();
            return book;
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

        [HttpGet("removeAccess/{bookId}/{courseId}")]
        public async Task<ActionResult<Book>> RemoveAccess(long bookId, long courseId)
        {
            var book = await _context.Books
                .Include(a => a.Chapters)
                    .ThenInclude(q => q.Topics)
                .Include(a => a.Courses)
                    .ThenInclude(q => q.Years)
                .FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null) return NotFound();
            var accessToRemove = book.Courses.SingleOrDefault(c => c.Id == courseId);
            if (accessToRemove == null) return NotFound();
            book.Courses.Remove(accessToRemove);

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return book;
        }

        [HttpGet("makeAccessibleToAll/{bookId}")]
        public async Task<ActionResult<Book>> MakeAccessibleToAll(long bookId)
        {
            var book = await _context.Books
                .Include(a => a.Chapters)
                .ThenInclude(q => q.Topics)
                .Include(a => a.Courses)
                .ThenInclude(q => q.Years)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null) return NotFound();
            
            book.AccessibleToAll = true;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return book;
        }
        
        [HttpGet("makeNotAccessibleToAll/{bookId}")]
        public async Task<ActionResult<Book>> MakeNotAccessibleToAll(long bookId)
        {
            var book = await _context.Books
                .Include(a => a.Chapters)
                .ThenInclude(q => q.Topics)
                .Include(a => a.Courses)
                .ThenInclude(q => q.Years)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null) return NotFound();
            
            book.AccessibleToAll = false;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return book;
        }

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