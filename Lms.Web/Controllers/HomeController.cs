using Lms.Core.Entities;
using Lms.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Lms.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            var course = await db.Courses.Include(c => c.Modules).ThenInclude(m => m.Activities).Where(c => c.Users.Contains(user)).FirstOrDefaultAsync();
            //var modules = course.Modules.ToList();
            //var activities = modules.
            //var model = new IndexCourseViewModel
            //{
            //    Modules = course.Modules.ToList(),
            //    Activities = 
            //};
            if (course == null)
            {
                return View();
            }

            return View(course);
        }
        public async Task<IActionResult> Activities(int moduleId)
        {
            var activities = await db.Activities.Where(a => a.ModuleId == moduleId).ToListAsync();

            return PartialView(activities);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}