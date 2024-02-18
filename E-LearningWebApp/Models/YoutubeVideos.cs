using System.ComponentModel.DataAnnotations;

namespace E_LearningWebApp.Models
{
    public class YoutubeVideos
    {
        [Key]
        public int VideoId { get; set; }
        public string VideoUrl { get; set; }
/*        public int SubCourseId { get; set; }
        public virtual SubCourses SubCourse { get; set; }*/
    }
}
