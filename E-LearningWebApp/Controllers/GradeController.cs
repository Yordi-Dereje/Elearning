using E_LearningWebApp.Areas.Identity.Data;
using E_LearningWebApp.Data;
using E_LearningWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_LearningWebApp.Controllers
{
    public class GradeController : Controller
    {
        private E_LearningDbContext _context = new E_LearningDbContext();
        private readonly UserManager<E_LearningWebAppUser> _userManager;
        private E_LearningWebAppContext _appContext;
        public GradeController(UserManager<E_LearningWebAppUser> _userManager, E_LearningWebAppContext _appContext)
        {
            this._userManager = _userManager;
            this._appContext = _appContext;

        }
        public IActionResult Index()
        {
            return View();
        }
/*
        [HttpPost]
        public ActionResult SetGrade(int courseID) // Assuming the value is a number
        {
            

        }*/

        
    }
}
