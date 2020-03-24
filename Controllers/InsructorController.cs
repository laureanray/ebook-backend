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
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        private readonly IAdminService _adminService;

        public InstructorController( 
            IAdminService adminService,
            IInstructorService instructorService)
        {
            _adminService = adminService;
            _instructorService = instructorService;
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