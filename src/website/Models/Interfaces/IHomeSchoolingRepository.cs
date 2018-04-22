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
        IQueryable<Mark> Marks { get; }

        void AddCourse(Course newCourse);
        void AddLesson(int courseId, Lesson newLesson);
        void AddHomeWork(int lessonId, Post newPost);
        void AddMark(int postId, int lessonId, int markValue, User teacher);
        void AddCourseListener(CoursesListener newListener);
        void AcceptCourse(string studentName, int courseId);
        void RefuseCourse(string studentName, int courseId);
        void AddUser(User toAdd);
        void SetTeacher(string name);
        void DeleteCourse(int courseId);

    }
}
