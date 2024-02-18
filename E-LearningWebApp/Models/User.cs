using System.ComponentModel.DataAnnotations;

namespace E_LearningWebApp.Models
{
    public class User
    {

        [Key]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

        [Required]
        [Display(Name = "Upload Cover")]
        public string imagePath { get; set; }

    }
}
