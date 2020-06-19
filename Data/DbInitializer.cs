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
                        Password = "1234",
                        StudentNumber = "20151234",
                        Year = "1st",
                        Section = "1",
                        Course = "BSCPE",
                        DateCreated = DateTime.Now,
                        FirstLogin = true
                    },
                    new Student
                    {    
                        FirstName = "John",
                        LastName = "Doe",
                        MiddleName = "Cruz",
                        Password = "1234",
                        Year = "1st",
                        Section = "2",
                        Course = "BSCPE",
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
                    Accesses = new List<Access>()
                    {
                        new Access()
                        {
                            Course = "BSCPE",
                            Year = "1st",
                        },
                        new Access()
                        {
                            Course = "BSCS",
                            Year = "1st"
                        }
                    },
                    Chapters = new List<Chapter>
                    {
                        new Chapter
                        {
                            ChapterTitle = "Chapter 1",
                            Exam = new Exam()
                            {
                                Instructions = "Test",
                                ExamItems = new List<ExamItem>()
                                {
                                    new ExamItem()
                                    {
                                        ExamType = "ID",
                                        Question = "Who developed C++?",
                                        Answer = "Bjarne Stroustrup"
                                    },
                                    new ExamItem()
                                    {
                                        ExamType = "ID",
                                        Question = "C++ first appeared in what year?",
                                        Answer = "1985"
                                    },
                                    new ExamItem()
                                    {
                                        ExamType = "MC",
                                        Question = "What is the abstraction Level of C++?",
                                        Answer = "Mid Level",
                                        Choices = new List<Choice>()
                                        {
                                            new Choice()
                                            {
                                                ChoiceText = "Low Level"
                                            },
                                            new Choice()
                                            {
                                                ChoiceText = "Mid Level"
                                            }, 
                                            new Choice()
                                            {
                                                ChoiceText = "High Level"
                                            }, 
                                            new Choice()
                                            {
                                                ChoiceText = "Bits"
                                            },
                                        }
                                    }
                                }
                            },
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
                    Accesses = new List<Access>()
                    {
                        new Access()
                        {
                            Course = "BSCPE",
                            Year = "1st",
                        },
                        new Access()
                        {
                            Course = "BSCS",
                            Year = "1st"
                        }
                    },
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
                }
            };


            foreach (var book in books)
            {
                Console.WriteLine("added book");
                context.Books.Add(book);
            }

            context.SaveChanges();

            if (!context.BookProgresses.Any())
            {
                var bookProgresses = new List<BookProgress>()
                {
                    new BookProgress()
                    {
                        Book = books[0],
                        Student = students[0],
                        LatestProgress = "0,0"
                    },
                    new BookProgress()
                    {
                        Book = books[1],
                        Student = students[0],
                        LatestProgress = "0,0"
                    },
                    new BookProgress()
                    {
                        Book = books[0],
                        Student = students[0],
                        LatestProgress = "0,0"
                    },
                    new BookProgress()
                    {
                        Book = books[1],
                        Student = students[1],
                        LatestProgress = "0,0"
                    }
                };

                foreach (var bp in bookProgresses)
                {
                    context.BookProgresses.Add(bp);
                    Console.WriteLine("Added");
                }

                context.SaveChangesAsync();
            }
            
            
                        //-------------- INSTRUCTOR ------------------
            if (context.Instructors.Any()) return;
            var instructors = new Instructor[]
            {
                new Instructor
                {
                    FirstName = "John",
                    LastName = "White",
                    MiddleName = "Walter",
                    Password = "1234",
                    Username = "john",
                    EmployeeNumber = "1112",
                    Honorifics = "Engr.",
                    DateCreated = DateTime.Now,
                    FirstLogin = true,
                    Assignments = new List<Assignment>()
                    {
                        new Assignment()
                        {
                            Book = books[0],
                            Course = "BSCS",
                            Year = "1st",
                            Section = "1"
                        },
                        new Assignment()
                        {
                            Book = books[0],
                            Course = "BSCS",
                            Year = "1st",
                            Section = "2"
                        },
                        new Assignment()
                        {
                            Book = books[1],
                            Course = "BSCS",
                            Year = "1st",
                            Section = "3"
                        },
                        new Assignment()
                        {
                            Book = books[0],
                            Course = "BSCS",
                            Year = "1st",
                            Section = "4"
                        }
                    }

                },
                new Instructor
                {
                    FirstName = "Samantha",
                    LastName = "Rodes",
                    MiddleName = "Wellington",
                    EmployeeNumber = "1113",
                    Honorifics = "Engr.",
                    Password = "1234",
                    Username = "sam",
                    DateCreated = DateTime.Now,
                    FirstLogin = true,
                    Assignments = new List<Assignment>()
                    {
                        new Assignment()
                        {
                            Book = books[1],
                            Course = "BSCPE",
                            Year = "1st",
                            Section = "1"
                        },
                        new Assignment()
                        {
                            Book = books[1],
                            Course = "BSCPE",
                            Year = "1st",
                            Section = "2"
                        },
                        new Assignment()
                        {
                            Book = books[1],
                            Course = "BSCPE",
                            Year = "1st",
                            Section = "3"
                        },
                        new Assignment()
                        {
                            Book = books[0],
                            Course = "BSCPE",
                            Year = "1st",
                            Section = "4"
                        }
                    }

                }
            };
            foreach (var instructor in instructors)
            {
                instructor.Password = BCrypt.Net.BCrypt.HashPassword(instructor.Password);
                context.Instructors.Add(instructor);
            }
            context.SaveChanges();
            //-------------- END ------------------
        }
        
        
        
    }
    
 
}