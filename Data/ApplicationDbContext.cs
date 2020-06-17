using System.Data.Common;
using ebook_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ebook_backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<BookProgress> BookProgresses { get; set; }
        public DbSet<Access> Accesses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
    }
}