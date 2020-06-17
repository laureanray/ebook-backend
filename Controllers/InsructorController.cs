using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ebook_backend.Data;
using ebook_backend.Models;
using ebook_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ebook_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        private readonly IAdminService _adminService;
        private readonly ApplicationDbContext _context;

        public InstructorController( 
            IAdminService adminService,
            IInstructorService instructorService, ApplicationDbContext context)
        {
            _adminService = adminService;
            _instructorService = instructorService;
            _context = context;
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Instructor>> GetInstructor(long id)
        {
            var instructor = await _context.Instructors.FirstOrDefaultAsync(s => s.Id == id);
            return instructor;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<Instructor>>> GetAll()
        {
            return await _context.Instructors.Include(a => a.Assignments).Where(i => i.IsArchived == false).ToListAsync();
        }
        
        
        [HttpGet("archived")]
        public async Task<ActionResult<List<Instructor>>> GetArchived()
        {
            return await _context.Instructors.Include(a => a.Assignments).Where(i => i.IsArchived == true).ToListAsync();
        }


        
        [HttpPost("add")]
        public async Task<ActionResult<Instructor>> AddInstructor([FromBody] Instructor instructor)
        {
            _context.Instructors.Add(instructor);
            instructor.Password = BCrypt.Net.BCrypt.HashPassword("1234");
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInstructor), new {id = instructor.Id}, instructor);
        }


        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<ActionResult<AdminInstructorResponse>> Authenticate([FromBody] Login login)
        {
            
            var response = new AdminInstructorResponse();

            var instructor = await _instructorService.Authenticate(login.UniqueIdentifier, login.Password);
            if (instructor != null)
            {
                response.Type = "Instructor";
                response.Instructor = instructor;
                return Ok(response);
            }

            var admin = await _adminService.Authenticate(login.UniqueIdentifier, login.Password);
            if (admin != null)
            {
                response.Type = "Admin";
                response.Admin = admin;
                return Ok(response);
            }

            return BadRequest(new {message = "Invalid Credentials"});
        }
        
        [HttpPost("update/{id}")]
        public async Task<ActionResult<Instructor>> UpdateInstructor(long id, [FromBody] Instructor instructor)
        {
            var instructorToUpdate = await _context.Instructors.FirstOrDefaultAsync(s => s.Id == id);
            if (instructorToUpdate == null) return NotFound();
            instructorToUpdate.FirstName = instructor.FirstName;
            instructorToUpdate.LastName = instructor.LastName;
            instructorToUpdate.MiddleName = instructor.MiddleName;
            instructorToUpdate.Password = BCrypt.Net.BCrypt.HashPassword(instructor.Password);
            instructorToUpdate.DateUpdated = DateTime.Now;
            _context.Entry(instructorToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return instructorToUpdate;
        }
        
        [HttpPost("update-password/{instructorId}/{newPassword}")]
        public async Task<ActionResult<Instructor>> UpdatePassword(string newPassword, long instructorId)
        {
            var instructorToUpdate = await _context.Instructors.FirstOrDefaultAsync(s => s.Id == instructorId);
            if (instructorToUpdate == null) return NotFound();
            instructorToUpdate.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.Entry(instructorToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return instructorToUpdate;
        }
        
        [HttpPost("archive/{instructorId}")]
        public async Task<ActionResult<Instructor>> Archive(long instructorId)
        {
            var instructor = await _context.Instructors.FirstOrDefaultAsync(s => s.Id == instructorId);
            if (instructor == null) return NotFound();
            instructor.IsArchived = true;
            _context.Entry(instructor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return instructor;
        }
        
        
        [HttpPost("restore/{instructorId}")]
        public async Task<ActionResult<Instructor>> Restore(long instructorId)
        {
            var instructor = await _context.Instructors.FirstOrDefaultAsync(s => s.Id == instructorId);
            if (instructor == null) return NotFound();
            instructor.IsArchived = false;    
            _context.Entry(instructor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return instructor;
        }

        [HttpPost("reset/{instructorId}")]
        public async Task<ActionResult<Instructor>> Reset(long instructorId)
        {
            var instructor = await _context.Instructors.FirstOrDefaultAsync(s => s.Id == instructorId);
            if (instructor == null) return NotFound();
            instructor.FirstLogin = true;
            instructor.Password = BCrypt.Net.BCrypt.HashPassword("1234");
            await _context.SaveChangesAsync();
            return instructor;
        }
    }

    
}