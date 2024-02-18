using E_LearningWebApp.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningWebApp.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; } // Renamed to follow convention
        public string PaymentStatus { get; set; }

        public int CourseId { get; set; } // Renamed to CourseId

        public string UserId { get; set; }
        public virtual E_LearningWebAppUser User { get; set; }

        public virtual Courses Course { get; set; } // Renamed to Course to match singular form of Courses

    }
}
