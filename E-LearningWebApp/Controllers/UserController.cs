using E_LearningWebApp.Areas.Identity.Data;
using E_LearningWebApp.Data;
using E_LearningWebApp.Models;
using E_LearningWebApp.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Principal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_LearningWebApp.Controllers
{
    public class UserController : Controller
    {
        private E_LearningDbContext _context = new E_LearningDbContext();
        private readonly UserManager<E_LearningWebAppUser> _userManager;
        private E_LearningWebAppContext _appContext;
        public UserController(UserManager<E_LearningWebAppUser> _userManager, E_LearningWebAppContext _appContext)
        {
            this._userManager = _userManager;
            this._appContext = _appContext;

        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }
        [HttpGet]
        public IActionResult ListUsers() 
        {
            var users = _userManager.Users.ToList();
            return View(users);

        }
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NorFound");
            }
            else
            {
                var result = await _userManager.UpdateAsync(user);
                return View(result);
            }
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NorFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                return View(result);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ProfileAsync([FromQuery] string userid)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound($"Unable to find '{userid}'.");
            }
            else
            {
                return View(user);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Profile([FromQuery] string userid, E_LearningWebAppContext userdata)
        {

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                _appContext.Update(userdata);
                _appContext.SaveChanges();
            }
            return RedirectToAction("Profile", "User");
        }
        public async Task<IActionResult> Payment( [FromForm] string userId,  [FromForm ]int courseId)
        {
            try
            {
                var course = await _context.Courses.FindAsync(courseId);
                var user = await _userManager.FindByIdAsync(userId);


                if ( course != null && user!= null)

                {
                    var pay = new Payment
                    {
                        PaymentStatus = "Paid",
                        CourseId = courseId,
                        UserId = userId

                    };
                    _context.Payments.Add(pay); // Assuming Payments is the DbSet for Payment entities
                    await _context.SaveChangesAsync(); // Save changes to the database
                }
                return RedirectToAction("Index", "Home"); // Redirect to an index page or similar
                

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, logging or displaying an error message
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        public IActionResult success()
        {
            return View();
        }


    }
}
