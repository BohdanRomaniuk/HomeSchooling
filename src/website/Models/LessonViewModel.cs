using database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models
{
    public class LessonViewModel
    {
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public Post LessonDescription { get; set; }
        public Post HomeWorkDescription { get; set; }

        public DateTime LessonStartDate { get; set; }
        public DateTime LessonEndDate { get; set; }
        public DateTime HomeWorkEnd { get; set; }

        public List<Mark> Marks { get; set; }
        public List<Post> HomeWorks { get; set; }
        public bool IsControlWork { get; set; }
        public LessonViewModel(Lesson currentLesson, List<Mark> marks)
        {
            LessonId = currentLesson.Id;
            LessonName = currentLesson.Name;
            LessonStartDate = currentLesson.LessonStartDate;
            LessonEndDate = currentLesson.LessonEndDate;
            HomeWorkEnd = currentLesson.HomeWorkEnd;
            IsControlWork = currentLesson.IsControlWork;
            LessonDescription = currentLesson.Posts.Where(s => s.PostType == "lesson-desc").SingleOrDefault();
            Marks = marks;
            HomeWorkDescription = currentLesson.Posts.Where(s => s.PostType == "homework-desc").SingleOrDefault();
            HomeWorks = currentLesson.Posts.Where(s => s.PostType == "homework").ToList();
        }
    }
}
