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

namespace ebook_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _studentService.GetAll();
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
        
    }
}    