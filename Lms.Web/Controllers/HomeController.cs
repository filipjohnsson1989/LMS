using Lms.Core.Entities;
using Lms.Web.Models;
using Lms.Web.Models.ViewModels;
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
            if (user != null)
            {

            }
            var course = await db.Courses.Where(c => c.Users.Any(u => u.Id == user.Id)).Include(c => c.Modules).ThenInclude(m => m.Activities).FirstOrDefaultAsync();
            if(course == null) throw new NullReferenceException(nameof(course));
            var model = new IndexCourseViewModel
            {
                Modules = course.Modules,
                Name = course.Name
            };

            if (course == null)
            {
                return View();
            }

            return View(model);
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