using Microsoft.AspNetCore.Mvc;
using E_LearningWebApp.Models;
using E_LearningWebApp.Repository;
using E_LearningWebApp.Data;
using Microsoft.AspNetCore.Identity;
using E_LearningWebApp.Areas.Identity.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using NuGet.Protocol;
using System;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;

namespace E_LearningWebApp.Controllers
{
    public class CourseController : Controller

    {
        private IWebHostEnvironment webHostEnvironment;
        private E_LearningDbContext _context = new E_LearningDbContext();

        private readonly UserManager<E_LearningWebAppUser> _userManager;
        private readonly SignInManager<E_LearningWebAppUser> _signInManager;

        public CourseController(UserManager<E_LearningWebAppUser> _userManager, IWebHostEnvironment webHostEnvironment, SignInManager<E_LearningWebAppUser> _signInManager)
        {
            this._userManager = _userManager;
            this.webHostEnvironment = webHostEnvironment;
            this._signInManager = _signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateCourse()
        {
            return View();
        }
        public IActionResult Payement()
        {
            return View();
        }
        public IActionResult Courses()
        {
            return View();
        }
        public IActionResult ConceptsAdmin()
        {
            return View();
        }
        public IActionResult ConceptsUser()
        {
            return View();
        }

        [HttpGet("CourseDisplay")]
        public IActionResult CourseDisplay()
        {
            CourseRepository pr = new CourseRepository(_context);
            List<Courses> courses = pr.GetAllCourses();

            return View(courses);
        }
        [HttpGet]
        public IActionResult CourseDisplayUser()
        {
            CourseRepository pr = new CourseRepository(_context);
            List<Courses> courses = pr.GetAllCourses();

            return View(courses);
        }

        [HttpGet]

        public async Task<IActionResult> CourseDetail([FromQuery] int courseId)
        {
            // Find the course asynchronously
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound(); // Return a  404 Not Found if the course is not found
            }

            // Initialize the repository and get the subcourses
            SubCourseRepository scr = new SubCourseRepository(_context);
            List<SubCourses> subcourses = scr.GetAllSubCourse(courseId);

            // Use the course and subcourses to construct the view model
            var multiplViewCourseSubCourseModel = from app in new[] { course } // Wrap the course object in an array to create a sequence
                                                  join subcourse in subcourses on app.CourseId equals subcourse.CourseId into table1
                                                  from subcourse in table1.DefaultIfEmpty() // Corrected spelling: DefaultIfEmpty
                                                  select new MultipleViewModel { courseview = app, subcourseview = subcourses };


            // Pass the view model to the view
            return View(multiplViewCourseSubCourseModel.FirstOrDefault()); // FirstOrDefault ensures a null is handled gracefully if no matching subcourses
        }

        [HttpGet]
        public async Task<IActionResult> CourseDetailUser([FromQuery] int courseId)
        {
            // Find the course asynchronously
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound(); // Return a  404 Not Found if the course is not found
            }

            // Initialize the repository and get the subcourses
            SubCourseRepository scr = new SubCourseRepository(_context);
            List<SubCourses> subcourses = scr.GetAllSubCourse(courseId);

            // Use the course and subcourses to construct the view model
            var multiplViewCourseSubCourseModeluser = from app in new[] { course } // Wrap the course object in an array to create a sequence
                                                      join subcourse in subcourses on app.CourseId equals subcourse.CourseId into table2
                                                      from subcourse in table2.DefaultIfEmpty() // Corrected spelling: DefaultIfEmpty
                                                      select new MultipleViewModel { courseview = app, subcourseview = subcourses };
            ViewBag.userid = _userManager.GetUserId(HttpContext.User);

            // Pass the view model to the view
            return View(multiplViewCourseSubCourseModeluser.FirstOrDefault()); // FirstOrDefault ensures a null is handled gracefully if no matching subcourses
        }
        public IActionResult Concepts()
        {
            return View();
        }
        [HttpPost]
        /*  public IActionResult CreateCourse(Courses course)--it works perfectly i commented it coz i updated the model
          {
              CourseRepository cr = new CourseRepository(_context);

              if (!ModelState.IsValid)
                  return View("CreateCourse");

              cr.AddCourse(course);
              return RedirectToAction("GetAllCourses");
          }*/
        public IActionResult CreateCourse(CourseViewModel cvm)
        {
            string fileName = "";
            string uniqueCFileName = "";
            string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");//url it combines the root url path with the folder name images--- wwwroot/Images
            if (cvm.imagePath != null)
            {
                fileName = Guid.NewGuid().ToString() + "-" + cvm.imagePath.FileName;
                uniqueCFileName = Path.Combine(uploadFolder, fileName);//wwwroot/Images/filename(guid+actual file name)
                cvm.imagePath.CopyTo(new FileStream(uniqueCFileName, FileMode.Create));//creates uniqueCFileName on server and copies the items
            }
            var course = new Courses()
            {
                CourseName = cvm.CourseName,
                CourseCode = cvm.CourseCode,
                CourseDescription = cvm.CourseDescription,
                CourseDuration = cvm.CourseDuration,
                CoursePrice = cvm.CoursePrice,
                imagePath = "/Images/" + fileName
            };
            CourseRepository cr = new CourseRepository(_context);

            if (!ModelState.IsValid)
                return View("CreateCourse");

            cr.AddCourse(course);
            return RedirectToAction("GetAllCourses");
        }


        [HttpGet("GetAllCourses")]
        public IActionResult GetAllCourses()
        {
            CourseRepository pr = new CourseRepository(_context);
            List<Courses> courses = pr.GetAllCourses();
            return View(courses);
        }
        [HttpGet]
        public async Task<IActionResult> GetCourseById([FromQuery] int courseid)
        {
            var course = await _context.Courses.FindAsync(courseid);
            ViewBag.CourseId = courseid;
            return View(course);

        }
        /*     [HttpGet("editCourses/{courseid}")]-- replace if the view and crud arent working
              public async Task<IActionResult> EditCourses(int courseid)
             {
                 var course = await _context.Courses.FindAsync(courseid);
                 return View(course);
             }

             [HttpPost("editCourses/{courseid}")]
              public IActionResult EditCourses(int courseid, Courses UpdatedCourse)
             {
                 var courses = _context.Courses.AsNoTracking().FirstOrDefaultAsync(course => course.CourseId == courseid);

                 if (courses != null)
                 {
                     _context.Update(UpdatedCourse);
                     _context.SaveChanges();
                 }

                 return RedirectToAction("GetAllCourses", "Course");
             }
     */
        [HttpGet("editCourses/{courseid}")]
        public async Task<IActionResult> EditCourses(int courseid)
        {
            var course = await _context.Courses.FindAsync(courseid);
            if (course == null)
            {
                return NotFound();
            }

            // Map the course entity to your view model
            var courses = new Courses
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseCode = course.CourseCode,
                CourseDescription = course.CourseDescription,
                CourseDuration = course.CourseDuration,
                CoursePrice = course.CoursePrice,
                imagePath = course.imagePath
            };

            return View(courses);
        }
        [HttpPost("editCourses/{courseid}")]
        public async Task<IActionResult> EditCourses(int courseid, CourseViewModel updatedCourseViewModel)
        {
            var course = await _context.Courses.FindAsync(courseid);
            if (course == null)
            {
                return NotFound();
            }

            if (updatedCourseViewModel.imagePath != null)
            {
                // Delete the old image file if a new one is uploaded
                System.IO.File.Delete(Path.Combine(webHostEnvironment.WebRootPath, "Images", course.imagePath));

                // Save the new image file
                string fileName = Guid.NewGuid().ToString() + "-" + updatedCourseViewModel.imagePath.FileName;
                string uniqueCFileName = Path.Combine(webHostEnvironment.WebRootPath, "Images", fileName);
                updatedCourseViewModel.imagePath.CopyTo(new FileStream(uniqueCFileName, FileMode.Create));

                // Update the image path in the course entity
                course.imagePath = "/Images/" + fileName;
            }

            // Update the other fields
            course.CourseName = updatedCourseViewModel.CourseName;
            course.CourseCode = updatedCourseViewModel.CourseCode;
            course.CourseDescription = updatedCourseViewModel.CourseDescription;
            course.CourseDuration = updatedCourseViewModel.CourseDuration;
            course.CoursePrice = updatedCourseViewModel.CoursePrice;

            // Save changes to the database
            _context.Update(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetAllCourses", "Course");
        }


        [HttpGet("deleteCourse/{courseid}")]
        public async Task<IActionResult> DeleteCourse(int courseid)
        {
            var course = await _context.Courses.FindAsync(courseid);
            return View(course);
        }

        [HttpPost("deleteCourse/{courseid}")]
        public IActionResult DeleteCourse(int courseid, Courses deletedcourse)
        {
            var courses = _context.Courses.AsNoTracking().FirstOrDefaultAsync(course => course.CourseId == courseid);

            if (courses != null)
            {
                _context.Remove(deletedcourse);
                _context.SaveChanges();
            }

            return RedirectToAction("GetAllCourses", "Course");
        }
        /*all above are working i dare you to touch it and die*/
        public IActionResult Webdevbasics()
        {
            return View();
        }
        public IActionResult Webdevadv()
        {
            return View();
        }
        public IActionResult Codingbasics()
        {
            return View();
        }
        public IActionResult Marketing()
        {
            return View();
        }
        public IActionResult Leadership()
        {
            return View();
        }
        public IActionResult NoCourse()
        {
            return View();
        }

        /*[HttpPost]
        public ActionResult SetGrade(double value, int courseID) // Assuming the value is a number
        {

            return Json(new { success = true, message = "Value received successfully" });
        }*/
        public IActionResult Webdevbasicsquiz()
        {
            return View();
        }
        public IActionResult Webdevadvquiz()
        {
            return View();
        }
        public IActionResult markquiz()
        {
            return View();
        }
        public IActionResult codbasquiz()
        {
            return View();
        }
        public IActionResult leaderquiz()
        {
            return View();
        }
        public IActionResult GenerateCertificate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SetGrade(double score, int courseID) // Assuming the value is a number
        {
            var user = await _userManager.GetUserAsync(User);
            var fullName = user.FirstName + " " + user.LastName;
            var courseName = "";
            if (courseID == 1)
            {
                courseName = "Web Development Basics";
            }
            else if (courseID == 2)
            {
                courseName = "Web Development Advanced";
            }
            string htmlTemplate = System.IO.File.ReadAllText("~/Content/Certificate.html");
            htmlTemplate = htmlTemplate.Replace("{{name}}", fullName);
            htmlTemplate = htmlTemplate.Replace("{{course}}", courseName);

            using MemoryStream ms = new MemoryStream();

            // Create a new PDF writer with the memory stream
            using PdfWriter writer = new PdfWriter(ms);

            // Create a new PDF document
            using PdfDocument document = new PdfDocument(writer);
            using Document doc = new Document(document);

            // Add the HTML content to the document
            Paragraph p = new Paragraph(htmlTemplate).SetWidth(UnitValue.CreatePercentValue(100));
            doc.Add(p);

            // Close the document to finalize the PDF creation
            document.Close();

            // Get the byte array from the memory stream
            byte[] pdfBytes = ms.ToArray();

            return File(pdfBytes, "application/pdf", "Certificate.pdf");

        }

    }
}

