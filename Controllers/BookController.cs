using System;
using System.Collections.Generic;
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

        [HttpPost("/add")]
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
            return await _context.Books.Include(b => b.Chapters).ThenInclude(a => a.Topics).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(long id)
        {
            var book = await _context.Books
                .Include(b => b.Chapters).ThenInclude(a => a.Topics).FirstOrDefaultAsync(a => a.Id == id);
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

            _context.Topics.Add(topic);

            await _context.SaveChangesAsync();

            return topic;
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


    }
}