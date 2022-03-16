using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Views.Home.Components.ComponentClasses
{
    public class ModulesVC : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ModulesVC(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var modules = await db.Modules.Where(module => module.CourseId == user.CourseId).ToListAsync();
            return View(modules);
        }
    }
}
