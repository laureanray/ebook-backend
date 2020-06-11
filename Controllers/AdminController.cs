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
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AdminService _adminService;

        public AdminController(ApplicationDbContext context, AdminService adminService)
        {
            _context = context;
            _adminService = adminService;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<ActionResult<Admin>> Authenticate([FromBody] Login login)
        {
            var admin = await _adminService.Authenticate(login.UniqueIdentifier, login.Password);

            if (admin == null) return BadRequest();

            return Ok(admin);
        }

        [HttpPost("update-password")]
        public async Task<ActionResult<Admin>> UpdatePassword([FromBody] string newPassword, [FromBody] long adminId)
        {
            var adminToUpdate = await _context.Admins.FirstOrDefaultAsync(s => s.Id == adminId);

            if (adminToUpdate == null) return NotFound();

            adminToUpdate.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            _context.Entry(adminToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return adminToUpdate;
        }


    }
}