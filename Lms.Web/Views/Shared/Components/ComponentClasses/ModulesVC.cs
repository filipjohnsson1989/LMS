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

            if (User.IsInRole("Teacher"))
            {
                int courseId;
                if (TempData.Peek("CourseId") is null)
                    courseId = iuw.courseRepo.GetAllCourses().Result.First().Id;
                else
                    courseId = int.Parse(TempData["CourseId"].ToString());

                TempData.Keep("CourseId");
                var modules = await iuw.moduleRepo.GetModulesByCourseIdAsync(courseId);
                return View(modules);
            }
            if (user != null && User.IsInRole("Student"))
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
