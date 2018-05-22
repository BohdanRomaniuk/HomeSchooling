using database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models
{
    public class HomeSchoolingRepository : IHomeSchoolingRepository
    {
        private HomeSchoolingContext context;

        public HomeSchoolingRepository(HomeSchoolingContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Course> Courses => context.Courses;
        public IQueryable<User> Users => context.Users;
        public IQueryable<Lesson> Lessons => context.Lessons;
        public IQueryable<CoursesListener> CoursesListeners => context.CoursesListeners;
        public IQueryable<Post> Posts => context.Posts;
        public IQueryable<Attachment> Attachments => context.Attachments;
        public IQueryable<Mark> Marks => context.Marks;

        public void AddCourse(Course newCourse)
        {
            context.Courses.Add(newCourse);
            context.SaveChanges();
        }

        public void AddLesson(int courseId, Lesson newLesson)
        {
            Course currentCourse = context.Courses.Include(c => c.CourseLessons).Where(c => c.Id == courseId).FirstOrDefault();
            currentCourse.CourseLessons.Add(newLesson);
            context.SaveChanges();
        }

        public void AddHomeWork(int lessonId, Post newPost)
        {
            Lesson currentLesson = context.Lessons.Where(l => l.Id == lessonId).SingleOrDefault();
            currentLesson.Posts.Add(newPost);
            context.SaveChanges();
        }

        public void AddMark(int postId, int lessonId, int markValue, User teacher)
        {
            Post currentPost = context.Posts.Include(p=>p.PostedBy).Include(p=>p.PostMark).Where(p => p.Id == postId).FirstOrDefault();
            Lesson currentLesson = context.Lessons.Where(l => l.Id == lessonId).FirstOrDefault();
            if(currentPost.PostMark==null)
            {
                currentPost.PostMark = new Mark(currentPost.PostedBy, teacher, markValue, DateTime.Now, currentLesson);
            }
            else
            {
                currentPost.PostMark.MarkValue = markValue;
                currentPost.PostMark.Teacher = teacher;
                currentPost.PostMark.MarkDate = DateTime.Now;
            }
            context.SaveChanges();
        }

        public void AddCourseListener(CoursesListener newListener)
        {
            context.CoursesListeners.Add(newListener);
            context.SaveChanges();
        }

        public void AcceptCourse(string studentName, int courseId)
        {
            CoursesListener listener = (from listeners in context.CoursesListeners
                                        where listeners.Student.UserName == studentName && listeners.RequestedCourse.Id == courseId
                                        select listeners).SingleOrDefault();
            listener.Accepted = true;
            context.SaveChanges();
        }

        public void RefuseCourse(string studentName, int courseId)
        {
            CoursesListener listener = (from listeners in context.CoursesListeners
                                        where listeners.Student.UserName == studentName && listeners.RequestedCourse.Id == courseId
                                        select listeners).SingleOrDefault();
            context.CoursesListeners.Remove(listener);
            context.SaveChanges();
        }

        public void AddUser(User toAdd)
        {
            context.Users.Add(toAdd);
            context.SaveChanges();
        }

        public void SetTeacher(string name)
        {
            //User teacher = context.Users.Where(u => u.UserName == name).SingleOrDefault();
            
            //teacher.UserRole = "teacher";
            //context.SaveChanges();
        }

        public void DeleteCourse(int courseId)
        {
            Course course = context.Courses.Include(c => c.CourseLessons).ThenInclude(l => l.Posts).Where(c => c.Id == courseId).SingleOrDefault();
            var listeners = context.CoursesListeners.Where(c => c.RequestedCourse.Id == courseId);
            foreach (var l in listeners)
            {
                context.CoursesListeners.Remove(l);
            }
            foreach (var l in course.CourseLessons)
            {
                foreach (var p in l.Posts)
                {
                    context.Posts.Remove(p);
                }
                context.Lessons.Remove(l);
            }
            context.Remove(course);
            context.SaveChanges();
        }

        public void ApproveUserToSystem(string username)
        {
            var selecteduser = from user in context.Users
                               where user.UserName == username
                               select user;
            if (selecteduser != null)
            {
                selecteduser.SingleOrDefault().Approved = true;
                context.SaveChanges();
            }
        }
        public void RejectUserToSystem(string username)
        {
            var selecteduser = from user in context.Users
                               where user.UserName == username
                               select user;
            if (selecteduser != null)
            {
                context.Remove(selecteduser.FirstOrDefault());
                context.SaveChanges();
            }
        }
    }
}
