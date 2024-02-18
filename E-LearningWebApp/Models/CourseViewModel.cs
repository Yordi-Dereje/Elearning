using System.ComponentModel.DataAnnotations;

namespace E_LearningWebApp.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Name is a mandatory")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Code is a mandatory")]
        public string CourseCode { get; set; }

        [Required]
        public string CourseDescription { get; set; }

        [Required(ErrorMessage = "Price is a mandatory")]
        public int CoursePrice { get; set; }

        [Required]
        public string CourseDuration { get; set; }
        [Display(Name = "Upload Image")]
        public IFormFile imagePath { get; set; }

       /* [Display(Name = "Upload Course Images")]
        public List<IFormFile> imageGallery { get; set; }//upload mutiple images from the user interface*/
    }
}
