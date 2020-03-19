using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ebook_backend.Data;
using ebook_backend.Models;
using ebook_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ebook_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ApplicationDbContext _context;

        public StudentController(IStudentService studentService, ApplicationDbContext context)
        {
            _studentService = studentService;
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }
        
        [AllowAnonymous]
        [Route("auth")]
        [HttpPost]
        public async Task<ActionResult<Student>> Authenticate([FromBody] Login login)
        {
            var student = await _studentService.Authenticate(login.UniqueIdentifier, login.Password);
            
            if (student == null)
            {
                return BadRequest(new {message = "Invalid Credentials"});
            }    
      
            return Ok(student);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(long id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            return student;
        }


        [HttpPost("add")]
        public async Task<ActionResult<Student>> AddStudent([FromBody] Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new {id = student.Id}, student);
        }

        [HttpPost("update/{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(long id, [FromBody] Student student)
        {
            var studentToUpdate = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);

            if (studentToUpdate == null) return NotFound();

            studentToUpdate.FirstName = student.FirstName;
            studentToUpdate.LastName = student.LastName;
            studentToUpdate.Password = BCrypt.Net.BCrypt.HashPassword(student.Password);
            studentToUpdate.DateUpdated = DateTime.Now;

            _context.Entry(studentToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return studentToUpdate;
        }
    }
}    