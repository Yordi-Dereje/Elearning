using E_LearningWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_LearningWebApp.Controllers
{
    public class CertificateController : Controller
    {
        public CertificateController() { }
        public ActionResult GenerateCertificate()
        {
            // Assuming you have the user's full name and course name
            string fullName = "John Doe";
            string courseName = "Web Development";

            // Create an instance of the model and set values
            var certificateModel = new CertificateViewModel
            {
                FullName = fullName,
                CourseName = courseName
            };

            // Pass the model to the view
            return View(certificateModel);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
