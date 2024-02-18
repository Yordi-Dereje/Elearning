using E_LearningWebApp.Areas.Identity.Data;

namespace E_LearningWebApp.Models
{
    public class AdminDashViewModel
    {
        public List<Courses> courseview { get; set; }
        public List<SubCourses> subcourseview { get; set; }
        public List<E_LearningWebAppUser> e_LearningWebAppUsers { get; set; }
        public List<Payment> payments { get; set; }
    }
}
