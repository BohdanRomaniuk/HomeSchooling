using System;
using Microsoft.EntityFrameworkCore;
using database.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace database
{
    public class Repository : IdentityDbContext<User>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<CoursesListener> CoursesListeners { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        public Repository()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=(LocalDb)\MSSQLLocalDB;initial catalog=HomeSchooling;integrated security=True;MultipleActiveResultSets=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<Lesson>().ToTable("Lessons");
            //modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<CoursesListener>().ToTable("CoursesListeners");
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Attachment>().ToTable("Attachments");
        }

        public static async Task CreateAccount(IServiceProvider serviceProvider, string username, string password, string email, string role)
        {
            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
                User user = new User
                {
                    UserName = username,
                    Email = email
                };
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }

    public class Database
    {
        public static void Main(string[] args)
        {
            using (Repository db = new Repository())
            {
                ///////////////////////////////
                ////Roles
                ///////////////////////////////
                //db.Roles.Add(new IdentityRole { Name = "Student", NormalizedName="STUDENT" });
                //db.Roles.Add(new IdentityRole { Name = "Teacher", NormalizedName="TEACHER" });
                //db.Roles.Add(new IdentityRole { Name = "Admin", NormalizedName="ADMIN" });


                ///////////////////////////////
                ////Users
                ///////////////////////////////

                //db.Users.Add(new User() { UserName = "admin1", PasswordHash = "AQAAAAEAACcQAAAAEB15hWqeKHUsyO2PxFozVKnfYCZQ76V7HErYUV2LGJ2Sh9zJ3rnZyQpz64AClaB4Ug==", Email= "admin1@gmail.com", NormalizedUserName="ADMIN1", NormalizedEmail="ADMIN1@GMAIL.COM", LockoutEnabled=True });
                //db.Users.Add(new User() { UserName = "roman.parobiy", PasswordHash = "AQAAAAEAACcQAAAAEB15hWqeKHUsyO2PxFozVKnfYCZQ76V7HErYUV2LGJ2Sh9zJ3rnZyQpz64AClaB4Ug==", Email= "roman.parobiy@gmail.com", NormalizedUserName= "ROMAN.PAROBIY", NormalizedEmail= "ROMAN.PAROBIY@GMAIL.COM" });
                //db.Users.Add(new User() { UserName = "bohdan.romaniuk", Password = "123456", Name = "Романюк Богдан", UserRole = "student" });
                //db.Users.Add(new User() { UserName = "roman.parobiy", Password = "123456", Name = "Паробій Роман", UserRole = "student" });
                //db.Users.Add(new User() { UserName = "modest.radomskyy", Password = "123456", Name = "Радомський Модест", UserRole = "student" });
                //db.Users.Add(new User() { UserName = "anatoliy.muzychuk", Password = "123456", Name = "Музичук А.О.", UserRole = "teacher" });
                //db.Users.Add(new User() { UserName = "sviatoslav.tarasyuk", Password = "123456", Name = "Тарасюк С.І.", UserRole = "teacher" });
                //db.Users.Add(new User() { UserName = "svyatoslav.litynskyy", Password = "123456", Name = "Літинський С.В.", UserRole = "teacher" });
                //db.SaveChanges();

                User muzychuk = db.Users.Where(u => u.UserName == "anatoliy.muzychuk").SingleOrDefault();
                User tarasyuk = db.Users.Where(u => u.UserName == "sviatoslav.tarasyuk").SingleOrDefault();
                User litynskyy = db.Users.Where(u => u.UserName == "svyatoslav.litynskyy").SingleOrDefault();

                /////////////////////////////
                //Courses and Lessons
                /////////////////////////////
                db.Courses.Add(new Course("Програмування мовою C++", "Курс для студентів 1 курс ФПМІ", muzychuk));
                db.Courses.Add(new Course("Проектування програмних систем", "Курс проектування програмних систем з використанням ASP .NET Core MVC", muzychuk));
                db.Courses.Add(new Course("Математичний аналіз", "Річний курс для студентів першого курсу ФПМІ", tarasyuk));
                db.Courses.Add(new Course("Алгоритми і структури даних", "Початковий курс алгоритмів сортування, пошуку та структур даних(дерева, хештаблиці, розріджені матриці, стек, черга)", litynskyy));
                db.Courses.Add(new Course("Бази даних та інформаційні системи", "Курс для освоєння роботи з реляційними та нереляційними базами даних, орієнтований на студентів 3 курсу ФПМІ", litynskyy));
                db.SaveChanges();

                //C++ Course
                Course cpp = db.Courses.Include(c => c.CourseLessons).Where(c => c.Name == "Програмування мовою C++").SingleOrDefault();
                Lesson cpp_lesson1 = new Lesson("Вступ у мову програмування C++", Convert.ToDateTime("01.09.2017 08:30"), Convert.ToDateTime("01.09.2017 09:50"), Convert.ToDateTime("08.09.2017 09:50"));
                cpp_lesson1.Posts = new List<Post>();
                cpp_lesson1.Posts.Add(new Post("Розглянути історію винекнення мови C++", "lesson-desc", muzychuk, Convert.ToDateTime("01.09.2017 08:30")));
                cpp_lesson1.Posts.Add(new Post("1)Встановити Visual Studio \n2)Створити та запустити програму Hello World!", "homework-desc", muzychuk, Convert.ToDateTime("01.09.2017 08:30")));
                Lesson cpp_lesson2 = new Lesson("Типи змінних, синтаксис", Convert.ToDateTime("08.09.2017 08:30"), Convert.ToDateTime("08.09.2017 09:50"), Convert.ToDateTime("15.09.2017 09:50"));
                cpp_lesson2.Posts = new List<Post>();
                cpp_lesson2.Posts.Add(new Post("Розглянути основні типи даних мови програмування C++ та синтаксис", "lesson-desc", muzychuk, Convert.ToDateTime("08.09.2017 08:30")));
                cpp_lesson2.Posts.Add(new Post("1)Навчитися працюват з різними типама даних int,char,double,string", "homework-desc", muzychuk, Convert.ToDateTime("08.09.2017 08:30")));
                Lesson cpp_lesson3 = new Lesson("Оператори", Convert.ToDateTime("15.09.2017 08:30"), Convert.ToDateTime("15.09.2017 09:50"), Convert.ToDateTime("22.09.2017 09:50"));
                cpp_lesson3.Posts = new List<Post>();
                cpp_lesson3.Posts.Add(new Post("Розглянути основні типи операторів мови програмування C++ та їх синтаксис", "lesson-desc", muzychuk, Convert.ToDateTime("15.09.2017 08:30")));
                cpp_lesson3.Posts.Add(new Post("1)Навчитися працюват з різними операторами.\n2)Написати програму, яка розвязуватиме квадратне рівняння", "homework-desc", muzychuk, Convert.ToDateTime("15.09.2017 08:30")));
                Lesson cpp_lesson4 = new Lesson("Робота з циклами", Convert.ToDateTime("22.09.2017 08:30"), Convert.ToDateTime("22.09.2017 09:50"), Convert.ToDateTime("29.09.2017 09:50"));
                cpp_lesson4.Posts = new List<Post>();
                cpp_lesson4.Posts.Add(new Post("Розглянути основні типи циклів мови програмування C++ та їх синтаксис", "lesson-desc", muzychuk, Convert.ToDateTime("22.09.2017 08:30")));
                cpp_lesson4.Posts.Add(new Post("1)Написати програму для обрахунку суми n елементів введених з консолі", "homework-desc", muzychuk, Convert.ToDateTime("22.09.2017 08:30")));

                cpp.CourseLessons.Add(cpp_lesson1);
                cpp.CourseLessons.Add(cpp_lesson2);
                cpp.CourseLessons.Add(cpp_lesson3);
                cpp.CourseLessons.Add(cpp_lesson4);

                //PPS Course
                Course pps = db.Courses.Include(c => c.CourseLessons).Where(c => c.Name == "Проектування програмних систем").SingleOrDefault();
                Lesson pps_lesson1 = new Lesson("Вступ у ASP .NET Core MVC", Convert.ToDateTime("05.03.2018 11:50"), Convert.ToDateTime("05.03.2018 13:10"), Convert.ToDateTime("12.03.2018 13:10"));
                pps_lesson1.Posts = new List<Post>();
                pps_lesson1.Posts.Add(new Post("Розповісти про історію винекнення ASP NET CORE MVC 2.0", "lesson-desc", muzychuk, Convert.ToDateTime("05.03.2018 11:50")));
                pps_lesson1.Posts.Add(new Post("1)Встановити фреймворк\n2)Виконати ch 2\n3)Придумати завдання для розробки власної системи\n4)Навчитися будувати uml - діаграми www.umlet.com \n5)У Chapters 8 - 10 продемонстровано прийоми, які можуть бути корисними для розробки ваших систем", "homework-desc", muzychuk, Convert.ToDateTime("05.03.2018 11:50")));

                Lesson pps_lesson2 = new Lesson("Use Case and Domain models", Convert.ToDateTime("12.03.2018 11:50"), Convert.ToDateTime("12.03.2018 13:10"), Convert.ToDateTime("19.03.2018 13:10"));
                pps_lesson2.Posts = new List<Post>();
                pps_lesson2.Posts.Add(new Post("Розповісти про Use Case and Domain models", "lesson-desc", muzychuk, Convert.ToDateTime("12.03.2018 11:50")));
                pps_lesson2.Posts.Add(new Post("1) Створити документ на drive.google.com і відкрити до ньго досутп всім членам команди\n2)У команді обговорити опис та призначенян проекту і записати його у документ\n3) Створити табличку для Use Cases, яка міститиме інформацію про самі Use Cases, дію яку вони виконоють та статус реалізації\n4)Створити діаграму", "homework-desc", muzychuk, Convert.ToDateTime("12.03.2018 11:50")));

                Lesson pps_lesson3 = new Lesson("Побудова підсистем", Convert.ToDateTime("19.03.2018 11:50"), Convert.ToDateTime("19.03.2018 13:10"), Convert.ToDateTime("27.03.2018 13:10"));
                pps_lesson3.Posts = new List<Post>();
                pps_lesson3.Posts.Add(new Post("Розповісти про історію винекнення ASP NET CORE MVC 2.0", "lesson-desc", muzychuk, Convert.ToDateTime("19.03.2018 11:50")));
                pps_lesson3.Posts.Add(new Post("1) Виконати 70% проекту", "homework-desc", muzychuk, Convert.ToDateTime("19.03.2018 11:50")));

                Lesson pps_lesson4 = new Lesson("Повна система 0-Release", Convert.ToDateTime("02.04.2018 11:50"), Convert.ToDateTime("02.04.2018 13:10"), Convert.ToDateTime("17.04.2018 13:10"));
                pps_lesson4.Posts = new List<Post>();
                pps_lesson4.Posts.Add(new Post("Розповісти про Software Desing(SD)", "lesson-desc", muzychuk, Convert.ToDateTime("01.04.2018 11:50")));
                pps_lesson4.Posts.Add(new Post("1) Виконати 100% проекту включно з тестами", "homework-desc", muzychuk, Convert.ToDateTime("01.04.2018 11:50")));

                pps.CourseLessons.Add(pps_lesson1);
                pps.CourseLessons.Add(pps_lesson2);
                pps.CourseLessons.Add(pps_lesson3);
                pps.CourseLessons.Add(pps_lesson4);
                db.SaveChanges();
            }
            Console.WriteLine("Database synhronized!");
            //Console.ReadKey();
        }
    }
}
