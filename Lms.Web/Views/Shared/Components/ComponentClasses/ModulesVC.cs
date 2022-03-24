using Microsoft.AspNetCore.Mvc;

namespace Lms.Web.Views.Shared.Components.ComponentClasses
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
          ////  if (teacher)
          //  {
          //      var modules = await iuw.moduleRepo.GetAllModulesByCourseId(//service.CourseId);
          //  }
            if (user != null)
            {
                var modules = await iuw.moduleRepo.GetModulesByCourseIdAsync((int)user.CourseId!);
                return View(modules);
            }
            else
            {
                throw new NullReferenceException(nameof(user));
            }

        }
    }
}
