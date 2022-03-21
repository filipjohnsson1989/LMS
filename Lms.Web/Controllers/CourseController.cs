using AutoMapper;
using Lms.Core.Entities;
using Lms.Core.Interfaces;
using Lms.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lms.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly IMapper mapper;

        //private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork uow;
        private readonly UserManager<ApplicationUser> userManager;

        public CourseController(IMapper mapper, IUnitOfWork unitofwork, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this.uow = unitofwork;
            this.userManager = userManager;
        }
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CourseOverView()
        {
            var userid =userManager.GetUserId(User);

            int courseid=uow.userRepo.GetCourse_By_UserId(userid);

            var course = await uow.courseRepo.GetAllbyId(courseid);
            return View(mapper.Map<CourseOverViewModel>(course));
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> LoadModulePartial(int id)
        {
            var course = await uow.courseRepo.GetAllbyId(id);
            return PartialView("_ModuleView", course);
        }
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> LoadStudentPartial(int id)
        {
            var course = await uow.courseRepo.GetAllbyId(id);
            return PartialView("_StudentView", course);
        }
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> LoadDocumentsPartial(int id)
        {
            var course = await uow.courseRepo.GetAllbyId(id);
            return PartialView("_DocumentView", course);
        }
    }
}
