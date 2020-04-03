using System.Collections.Generic;
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

        [HttpGet]
        public async Task<ActionResult<List<Instructor>>> GetAll()
        {
            return await _context.Instructors.ToListAsync();
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


    }
}