using System.Collections.Generic;
using System.Threading.Tasks;
using ebook_backend.Data;
using ebook_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ebook_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<List<Exam>> GetAllExams()
        {
            return await _context.Exams.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Exam>> GetExam(long id)
        {
            var exam = await _context.Exams.FirstOrDefaultAsync(e => e.Id == id);
            if (exam == null) return NotFound();
            return exam;
        }

        [HttpPost("add/{chapterId}")]
        public async Task<ActionResult<Exam>> AddExam([FromBody] Exam exam, long chapterId)
        {
            var chapter = await _context.Chapters.FirstOrDefaultAsync(a => a.Id == chapterId);
            if (chapter == null) return NotFound();
            chapter.Exam = exam;
            _context.Entry(chapter).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return exam;
        }

        [HttpPost("delete/{examId}")]
        public async Task<ActionResult<Exam>> RemoveExam(long examId)
        {
            var examToRemove = await _context.Exams.FirstOrDefaultAsync(e => e.Id == examId);
            if (examToRemove == null) return NotFound();
            _context.Exams.Remove(examToRemove);
            await _context.SaveChangesAsync();
            return examToRemove;
        }
        
         
        
    }
}