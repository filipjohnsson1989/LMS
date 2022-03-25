using Microsoft.AspNetCore.Mvc;

namespace Lms.Web.Views.Shared.Components.ComponentClasses
{
    public class ModulesVC : ViewComponent
    {
        private readonly IUnitOfWork iuw;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger logger;
        private static int count;

        public ModulesVC(IUnitOfWork iuw, UserManager<ApplicationUser> userManager, ILogger logger)
        {
            this.iuw = iuw;
            this.userManager = userManager;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            count++;
            logger.LogInformation($"\r\n\r\n\r\n\r\n ModuleVD Id: {TempData.Peek("CourseId")} Times:{count}\r\n\r\n\r\n\r\n\r\n");
            
            var user = await userManager.FindByIdAsync(userId);

            if (User.IsInRole("Teacher"))
            {
                int courseId;
                if(TempData.Peek("CourseId") is null)
                    courseId = iuw.courseRepo.GetAllCourses().Result.First().Id;
                else
                 courseId = int.Parse(TempData["CourseId"].ToString());

                
                TempData.Keep("CourseId");
                var modules = await iuw.moduleRepo.GetAllModulesByCourseId(courseId);
                return View(modules);
            }
            if (user != null && User.IsInRole("Student"))
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
