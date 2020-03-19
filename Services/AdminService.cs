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
    public interface IAdminService
    {
        Task<Admin> Authenticate(string studentNumber, string password);
        
    }

    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;

        public AdminService(ApplicationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        
        public async Task<Admin> Authenticate(string username, string password)
        {
            // throw new System.NotImplementedException();
            var admin = await _context.Admins.SingleOrDefaultAsync(
                x => x.Username == username);

            if (admin == null) return null;

            if(!BCrypt.Net.BCrypt.Verify(password, admin.Password)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, admin.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            admin.Token = tokenHandler.WriteToken(token);

            return admin;
        }

      
    }
}