using Microsoft.AspNetCore.Mvc;

namespace Lms.Web.Views.Home.Components.ComponentClasses
{
    public class ModulesVC : ViewComponent
    {
        private readonly IUnitOfWork iuw;
        private readonly UserManager<ApplicationUser> userManager;

        public ModulesVC(IUnitOfWork iuw, UserManager<ApplicationUser> userManager)
        {
            this.iuw = iuw;
            this.userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var modules = await iuw.moduleRepo.GetAllModulesByCourseId((int)user.CourseId!);
                return View(modules);
            }
            else
            {
                throw new NullReferenceException(nameof(user));
            }
            
        }
    }
}
