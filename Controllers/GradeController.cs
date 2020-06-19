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
    public class GradeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GradeController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> GetGrade(long id)
        {
            var grade = await _context.Grades.FirstOrDefaultAsync(s => s.Id == id);
            if (grade == null) return NotFound();
            return grade;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Grade>> AddGrade([FromBody] Grade grade)
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGrade), new {id = grade.Id}, grade);
        }
    }
}