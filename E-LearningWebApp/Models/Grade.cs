using E_LearningWebApp.Areas.Identity.Data;
using Newtonsoft.Json.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_LearningWebApp.Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        public string Userid { get; set; }
        public virtual E_LearningWebAppUser User { get; set; }
        public int SCId { get; set; }
        public int Value { get; set; }
    }
}
