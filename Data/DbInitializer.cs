using System;
using System.Linq;
using ebook_backend.Models;

namespace ebook_backend.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Students.Any())
            {
                return;
            }

            var students = new Student[]
            {
                new Student
                {
                    FirstName = "Juan",
                    LastName = "Dela Cruz",
                    MiddleName = "Santos",
                    Password = "P@$$w0rd",
                    StudentNumber = "20151234",
                    DateCreated = DateTime.Now
                },
                new Student
                {
                    FirstName = "Someone",
                    LastName = "Doe",
                    MiddleName = "Cruz",
                    Password = "P@$$w0rd",
                    StudentNumber = "20151134",
                    DateCreated = DateTime.Now
                }
            };

            foreach (var student in students)
            {
                context.Students.Add(student);
            }

            context.SaveChanges();
        }
    }
}