using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Views.Home.Components.ComponentClasses
{
    public class ActivitiesVC : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public ActivitiesVC(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int moduleId)
        {
            var activities = await db.Activities.Where(a => a.ModuleId == moduleId).ToListAsync();

            return View(activities);
        }
    }
}
