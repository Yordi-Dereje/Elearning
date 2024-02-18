using System.ComponentModel.DataAnnotations;

namespace E_LearningWebApp.Models
{
    public class Courses
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        public string CourseName { get; set; }

        [Required]
        public string CourseCode { get; set; }

        [Required]
        public string CourseDescription { get; set; }

        [Required]
        public int CoursePrice { get; set; }

        [Required]
        public string CourseDuration { get; set; }

        public string imagePath { get; set; }

        public List<SubCourses>? SubCourses { get; set;}
        public Payment Payment { get; set; } // Renamed to match singular form of Payment

    }
}
