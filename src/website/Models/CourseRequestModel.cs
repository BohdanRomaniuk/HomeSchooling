using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models
{
    public class CourseRequestModel
    {
        public int CourseId { set; get; }
        public string CourseName { set; get; }
        public string StudentName { set; get; }
        public string StudentUserName { set; get; }
        public int StudentId { set; get; }
        public CourseRequestModel(int _id, int _stud_id, string _name, string _studentname, string _studentuname)
        {
            CourseId = _id;
            CourseName = _name;
            StudentName = _studentname;
            StudentUserName = _studentuname;
            StudentId = _stud_id;
        }
    }
}
