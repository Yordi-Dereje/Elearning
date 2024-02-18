using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_LearningWebApp.Data;
using E_LearningWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_LearningWebApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the E_LearningWebAppUser class
public class E_LearningWebAppUser : IdentityUser
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }


    public string imagePath { get; set; }
    public List<Grade>? grades { get; set; }
    public virtual ICollection<Payment> payments { get; set; }

    /*[Required]
    [Display(Name = "User Role")]
    public string UserRole { get; set; }*/
}

