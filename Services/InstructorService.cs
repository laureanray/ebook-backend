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
    public interface IInstructorService
    {
        Task<Instructor> Authenticate(string studentNumber, string password);
        
    }

    public class InstructorService : IInstructorService
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;

        public InstructorService(ApplicationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        
        public async Task<Instructor> Authenticate(string username, string password)
        {
            // throw new System.NotImplementedException();
            var instructor = await _context.Instructors.SingleOrDefaultAsync(
                x => x.Username == username && !x.IsArchived);

            if (instructor == null) return null;

            if(!BCrypt.Net.BCrypt.Verify(password, instructor.Password)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, instructor.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            instructor.Token = tokenHandler.WriteToken(token);

            return instructor;
        }

      
    }
}