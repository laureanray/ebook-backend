using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ebook_backend.Data;
using ebook_backend.Helpers;
using ebook_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ebook_backend.Services
{
    public interface IStudentService
    {
        Task<Student> Authenticate(string studentNumber, string password);
        
    }

    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;

        public StudentService(ApplicationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        
        public async Task<Student> Authenticate(string studentNumber, string password)
        {
            // throw new System.NotImplementedException();
            var student = await _context.Students.Include(s => s.BookProgresses).SingleOrDefaultAsync(
                x => x.StudentNumber == studentNumber && !x.IsArchived);

            if (student == null) return null;

            if(!BCrypt.Net.BCrypt.Verify(password, student.Password)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, student.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            student.Token = tokenHandler.WriteToken(token);

            return student;
        }
        

      
    }
}