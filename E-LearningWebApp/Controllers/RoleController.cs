using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace E_LearningWebApp.Controllers
{
    public class RoleController : Controller
    {   
        RoleManager<IdentityRole> roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }
        //[Authorize(Policy = "Adminroles")]
        public IActionResult Create()
        {
            return View(new IdentityRole());
        }
        [HttpPost]
        //[Authorize(Policy = "Adminroles")]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            await roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }
    }
}
