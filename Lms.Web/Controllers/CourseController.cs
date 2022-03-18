using AutoMapper;
using Lms.Core.Interfaces;
using Lms.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lms.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly IMapper mapper;

        //private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork uow;
        public CourseController(IMapper mapper, IUnitOfWork unitofwork)
        {
            this.mapper = mapper;
            this.uow = unitofwork;
        }
        public async Task<IActionResult> CourseOverView(int courseid=1)
        {
            var course = await uow.courseRepo.GetAllbyId(courseid);
            return View(mapper.Map<CourseOverViewModel>(course));
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadModulePartial(int courseid=1)
        {
            var course = uow.courseRepo.GetAllbyId(courseid);
            return PartialView("_ModuleView", course);
        }

        public async Task<IActionResult> LoadStudentPartial(int courseid=1)
        {
            var course = await uow.courseRepo.GetAllbyId(courseid);
            return PartialView("_StudentView", course);
        }
        public async Task<IActionResult> LoadDocumentsPartial(int courseid=1)
        {
            var course = await uow.courseRepo.GetAllbyId(courseid);
            return PartialView("_DocumentView", course);
        }
    }
}
