using System.ComponentModel.DataAnnotations;

namespace E_LearningWebApp.Models
{
    public class SubCourses
    {
        [Key]
        public int SubCourseId { get; set; }
        public string SubCourseName { get; set;}
        public string SubCourseDescription { get; set;}
        public string SubCourseDuration { get; set;}
        public int CourseId { get; set; }
        public virtual Courses Course { get; set; }

    
     



    }
}
