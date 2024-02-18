using Microsoft.AspNetCore.Mvc;

namespace E_LearningWebApp.Controllers
{
    public class InstructorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
