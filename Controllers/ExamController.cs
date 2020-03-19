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

        [HttpPost("add")]
        public async Task<ActionResult<Exam>> AddExam([FromBody] Exam exam)
        {
            _context.Exams.Add(exam);

            await _context.SaveChangesAsync();

            return exam;
        }
        
         
        
    }
}