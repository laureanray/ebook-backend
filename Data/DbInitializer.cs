using System;
using System.Collections.Generic;
using System.Linq;
using ebook_backend.Models;
using Microsoft.AspNetCore.Identity;
using BCrypt.Net;
using Microsoft.AspNetCore.StaticFiles;

namespace ebook_backend.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            //-------------- STUDENTS ------------------
            if (context.Students.Any()) return;
            var students = new Student[]
                {
                    new Student
                    {
                        FirstName = "Juan",
                        LastName = "Dela Cruz",
                        MiddleName = "Santos",
                        Password = "P@$$w0rd",
                        StudentNumber = "20151234",
                        DateCreated = DateTime.Now,
                        FirstLogin = true
                    },
                    new Student
                    {
                        FirstName = "Someone",
                        LastName = "Doe",
                        MiddleName = "Cruz",
                        Password = "P@$$w0rd",
                        StudentNumber = "20151134",
                        DateCreated = DateTime.Now,
                        FirstLogin = true

                    }
                };
            foreach (var student in students)
            {
                student.Password = BCrypt.Net.BCrypt.HashPassword(student.Password);
                context.Students.Add(student);
            }
            context.SaveChanges();
            //-------------- END ------------------
            
            //-------------- INSTRUCTOR ------------------
            if (context.Instructors.Any()) return;
            var instructors = new Instructor[]
            {
                new Instructor
                {
                    FirstName = "Ruben",
                    LastName = "Pureza",
                    MiddleName = "Gerud",
                    Password = "1234",
                    Username = "ruben",
                    DateCreated = DateTime.Now,
                    FirstLogin = true

                },
                new Instructor
                {
                    FirstName = "Angela",
                    LastName = "Santos",
                    MiddleName = "Cruz",
                    Password = "1234",
                    Username = "angela",
                    DateCreated = DateTime.Now,
                    FirstLogin = true

                }
            };
            foreach (var instructor in instructors)
            {
                instructor.Password = BCrypt.Net.BCrypt.HashPassword(instructor.Password);
                context.Instructors.Add(instructor);
            }
            context.SaveChanges();
            //-------------- END ------------------
            
            //-------------- ADMIN ------------------
            if (context.Admins.Any()) return;
            var admin = new Admin
            {
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("1234"),
                DateCreated = DateTime.Now
            };
            context.Admins.Add(admin);
            context.SaveChanges();
            //-------------- END ------------------
            

            //-------------- BOOKS ------------------
            if(context.Books.Any()) return;
            var books = new Book[]
            {
                new Book
                {
                    BookTitle = "C++ Programming",
                    BookAuthor = "Juan Dela Cruz",
                    BookDescription = "Book Description",
                    DateCreated = DateTime.Now,
                    Chapters = new List<Chapter>
                    {
                        new Chapter
                        {
                            ChapterTitle = "Chapter 1",
                            Topics = new List<Topic>
                            {
                                new Topic
                                {
                                    TopicTitle = "Topic 1",
                                    HtmlContent = "<h1> Hello World </h1>"
                                },
                                new Topic
                                {
                                    TopicTitle = "Topic 2",
                                    HtmlContent = "<h1> Hello World </h1>"
                                },
                                new Topic
                                {
                                    TopicTitle = "Topic 3",
                                    HtmlContent = "<h1> Hello World </h1>"
                                },
                                new Topic
                                {
                                    TopicTitle = "Topic 4",
                                    HtmlContent = "<h1> Hello World </h1>"
                                }
                            }
                        }
                    }
                },
                new Book
                {
                    BookTitle = "Discrete Math",
                    BookAuthor = "Juan Dela Cruz",
                    BookDescription = "Book Description",
                    DateCreated = DateTime.Now,
                    Chapters = new List<Chapter>
                    {
                        new Chapter
                        {
                            ChapterTitle = "Chapter 2",
                            Topics = new List<Topic>
                            {
                                new Topic
                                {
                                    TopicTitle = "Topic 1",
                                    HtmlContent = "<h1> Hello World </h1>"
                                },
                                new Topic
                                {
                                    TopicTitle = "Topic 2",
                                    HtmlContent = "<h1> Hello World </h1>"
                                },
                                new Topic
                                {
                                    TopicTitle = "Topic 3",
                                    HtmlContent = "<h1> Hello World </h1>"
                                },
                                new Topic
                                {
                                    TopicTitle = "Topic 4",
                                    HtmlContent = "<h1> Hello World </h1>"
                                }
                            }
                        }
                    }
                }
            };


            foreach (var book in books)
            {
                context.Books.Add(book);
            }

            context.SaveChanges();
        }
        
    }
    
 
}