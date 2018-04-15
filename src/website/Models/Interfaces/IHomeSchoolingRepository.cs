using database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models
{
    public interface IHomeSchoolingRepository
    {
        IQueryable<Course> Courses { get; }
        IQueryable<User> Users { get; }
        IQueryable<Lesson> Lessons { get; }
        IQueryable<CoursesListener> CoursesListeners { get; }
        IQueryable<Post> Posts { get; }
        IQueryable<Attachment> Attachments { get; }

        void AddCourse(Course newCourse);
        void AddLesson(int courseId, Lesson newLesson);
        void AddHomeWork(int lessonId, Post newPost);
        void AddCourseListener(CoursesListener newListener);
        void AcceptCourse(int studentId, int courseId);
        void RefuseCourse(int studentId, int courseId);
        void AddUser(User toAdd);
        void SetTeacher(int userId);
        void DeleteCourse(int courseId);
    }
}
