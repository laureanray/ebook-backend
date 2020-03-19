using System.Threading.Tasks;
using ebook_backend.Data;
using ebook_backend.Models;
using ebook_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ebook_backend.Controllers
{
    [Authorize]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly InstructorService _instructorService;

        public InstructorController(ApplicationDbContext context, InstructorService instructorService)
        {
            _context = context;
            _instructorService = instructorService;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<ActionResult<Admin>> Authenticate([FromBody] Login login)
        {
            var admin = await _instructorService.Authenticate(login.UniqueIdentifier, login.Password);

            if (admin == null) return BadRequest();

            return Ok(admin);
        }


    }
}