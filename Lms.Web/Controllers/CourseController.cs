using AutoMapper;
using Lms.Core.Entities;
using Lms.Core.Interfaces;
using Lms.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
#nullable disable

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
        public async Task<IActionResult> Student_CourseOverview()
        {
            var user = await userManager.GetUserAsync(User);

            var course = await uow.courseRepo.GetAllbyId((int)user.CourseId);

            return View(mapper.Map<CourseOverViewModel>(course));
        }
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Teacher_CourseOverview()
        {

            var course = await uow.courseRepo.GetAllCourses();

            return View(course);
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
            //var course = await uow.courseRepo.GetAllbyId(id);
            var user =userManager.GetUserId(User);
            var document =  uow.documentRepo.GetDocumentBy_UserId(user);
            //var doc = mapper.Map<StudentDocumentViewModel>(document);
            var doc = mapper.Map<IEnumerable<StudentDocumentViewModel>>(document);

            return PartialView("_DocumentView", doc);
        }
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> LoadModules(int id)
        {
            var course = await uow.courseRepo.GetAllbyId(id);
            return View(course);
        }
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> EditCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await uow.courseRepo.GetCourseById((int)id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> EditCourse(int id, EditCourseModel course)
        {
            var courseobj = mapper.Map<Course>(course);
            if (id != courseobj.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    uow.courseRepo.UpdateCourse(courseobj);
                    await uow.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!uow.courseRepo.CourseExists(courseobj.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Teacher_CourseOverview));
            }
            return View(courseobj);
        }
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await uow.courseRepo.DeleteCourse(id);
            await uow.CompleteAsync();
            return RedirectToAction(nameof(Teacher_CourseOverview));
        }

        public IActionResult TestAction(string test)
        {
            return View("Index");
        }
    }
}
