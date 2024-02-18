using E_LearningWebApp.Areas.Identity.Data;
using E_LearningWebApp.Models;
using E_LearningWebApp.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_LearningWebApp.Controllers
{
    public class SubCourseController : Controller
    {
        private E_LearningDbContext _context = new E_LearningDbContext();
        //private readonly object webHostEnvironment;

        public SubCourseController()/*IWebHostEnvironment webHostEnvironment*/
        {
            //this.webHostEnvironment = webHostEnvironment;
        }

        /* ...works perfectly dont touch [HttpGet("GetAllSubCourses")]
         public IActionResult GetAllSubCourses()
         {
             SubCourseRepository scr = new SubCourseRepository(_context);
             List<SubCourses> subcourses = scr.GetAllSubCourse();
             return View(subcourses);
         }*/



        [HttpGet("GetAllSubCourses/{courseid}")]
        public IActionResult GetAllSubCourses(int courseid)
        {
            SubCourseRepository scr = new SubCourseRepository(_context);
            List<SubCourses> subcourses = scr.GetAllSubCourse(courseid);
            ViewBag.CourseId = courseid;
            return View(subcourses);
        }
        public IActionResult CreateSubCourse()
        {
            return View();
        }
        public IActionResult Git()
        {
            return View();
        }
        public IActionResult Flowchart()
        {
            return View();
        }
        public IActionResult Job()
        {
            return View();
        }
        public IActionResult Html()
        {
            return View();
        }
        public IActionResult Css()
        {
            return View();
        }
        public IActionResult Js()
        {
            return View();
        }
        public IActionResult Database()
        {
            return View();
        }
        public IActionResult Debug()
        {
            return View();
        }
        public IActionResult Mysql()
        {
            return View();
        }
        public IActionResult Py()
        {
            return View();
        }
        public IActionResult React()
        {
            return View();
        }
        public IActionResult Rust()
        {
            return View();
        }
        public IActionResult Shell()
        {
            return View();
        }
        public IActionResult Grades()
        {
            return View();
        }
        public IActionResult DigMark()
        {
            return View();
        }
        public IActionResult MarkRes()
        {
            return View();
        }
        public IActionResult Cons()
        {
            return View();
        }
        
        public IActionResult Sales()
        {
            return View();
        }
        public IActionResult Comm()
        {
            return View();
        }
        public IActionResult OB()
        {
            return View();
        }
        public IActionResult Strat()
        {
            return View();
        }
        public IActionResult Team()
        {
            return View();
        }
        public IActionResult EmInt()
        {
            return View();
        }
        public IActionResult ChangeMan()
        {
            return View();
        }


        [HttpGet]
        public IActionResult CreateSubCourse(int courseid)
        {
            CourseRepository pr = new CourseRepository(_context);
            Courses course = pr.GetCourseById(courseid);
            ViewBag.CourseId = course.CourseId;
            ViewBag.CourseName = course.CourseName;
            return View();
        }

        [HttpPost]
        public IActionResult CreateSubCourse(int courseId, SubCourses subcourse)
        {

            SubCourseRepository scr = new SubCourseRepository(_context);
            subcourse.CourseId = courseId;
            scr.AddSubCourse(subcourse);
            return RedirectToAction("CreateSubCourse", new
            {
                courseid = courseId
            });
        }



        [HttpGet]
        public async Task<IActionResult> GetSubCourseById([FromQuery] int subcourseid, [FromQuery] int courseid)
        {
            var subcourse = await _context.SubCourses.FindAsync(subcourseid);
            var course = await _context.Courses.FindAsync(courseid);

            ViewBag.CourseId = courseid;
            ViewBag.CourseName = course.CourseName;
            return View(subcourse);

        }
        [HttpGet("editSubCourse/{courseid}/{subcourseid}")]
        public async Task<IActionResult> EditSubCourse(int courseid, int subcourseid)
        {
            CourseRepository pr = new CourseRepository(_context);
            Courses course = pr.GetCourseById(courseid);
            ViewBag.CourseId = course.CourseId;
            ViewBag.CourseName = course.CourseName;
            var subcourse = await _context.SubCourses.FindAsync(subcourseid);
            return View(subcourse);
        }
        [HttpPost("editSubCourse/{courseid}/{subcourseid}")]
        public IActionResult EditSubCourseAsync(int courseid, int subcourseid, SubCourses UpdatedSubCourse)
        {

            var subcourses = _context.SubCourses.AsNoTracking().FirstOrDefaultAsync(subcourse => subcourse.SubCourseId == subcourseid && courseid == subcourse.CourseId);

            if (subcourses != null)
            {
                _context.Update(UpdatedSubCourse);
            }
            return RedirectToAction("GetAllSubCourses", "SubCourse", new { courseid });
        }

        [HttpGet("deleteSubCourse/{courseid}/{subcourseid}")]
        public async Task<IActionResult> DeleteSubCourse(int courseid, int subcourseid)
        {
            CourseRepository pr = new CourseRepository(_context);
            Courses course = pr.GetCourseById(courseid);
            ViewBag.CourseId = course.CourseId;
            ViewBag.CourseName = course.CourseName;
            var subcourse = await _context.SubCourses.FindAsync(subcourseid);
            return View(subcourse);
        }

        [HttpPost("deleteSubCourse/{courseid}/{subcourseid}")]
        public IActionResult DeleteSubCourse(int courseid, SubCourses deletedcourse, int subcourseid)
        {
            var subcourses = _context.SubCourses.AsNoTracking().FirstOrDefaultAsync(course => course.SubCourseId == subcourseid);

            if (subcourses != null)
            {
                _context.Remove(deletedcourse);
                _context.SaveChanges();
            }

            return RedirectToAction("GetAllSubCourses", "SubCourse", new { courseid });
        }

    }
}
