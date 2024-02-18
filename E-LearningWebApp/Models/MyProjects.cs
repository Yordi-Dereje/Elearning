using System.ComponentModel.DataAnnotations;

namespace E_LearningWebApp.Models
{
    public class MyProjects
    {
        [Key]
        public int MyProjectId { get; set; }
        public string ProjectStatus { get; set; }

    }
}
