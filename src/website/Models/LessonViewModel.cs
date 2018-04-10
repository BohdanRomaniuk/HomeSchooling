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
        public List<Post> HomeWorks { get; set; }
        public LessonViewModel(int lessonId, string lessonName, List<Post> allLessonPosts)
        {
            LessonId = lessonId;
            LessonName = lessonName;
            LessonDescription = allLessonPosts.Where(s => s.PostType == "lesson-desc").SingleOrDefault();
            HomeWorkDescription = allLessonPosts.Where(s => s.PostType == "homework-desc").SingleOrDefault();
            HomeWorks = allLessonPosts.Where(s => s.PostType == "homework").ToList();
        }
    }
}
