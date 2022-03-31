using AutoMapper;
using Lms.Core.ViewModels.Courses;
using Lms.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace Lms.Web.Controllers;

public class CoursesController : Controller
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IMapper mapper;
    private readonly ICourseSelector ser;

    public CoursesController(IUnitOfWork unitOfWork,
                             UserManager<ApplicationUser> userManager,
                             IMapper mapper, ICourseSelector ser)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
        this.mapper = mapper;
        this.ser = ser;
    }

    // Dynamic course id
    [HttpPost]
    [Route("CourseSelector/TempData/")]
    public ActionResult SetTempData(int id)
    {
        // Set your TempData key to the value passed in
        TempData["CourseId"] = id;
        TempData.Keep("CourseId");
        return Ok("success");
    }


    // GET: Courses
    public async Task<IActionResult> Index()
    {
        var courses = await unitOfWork.CourseRepoG
                                    .GetAllAsync();
        var coursesToReturn = mapper.Map<IEnumerable<CourseViewModel>>(courses);
        return View(coursesToReturn);
    }

    // GET: Courses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();

        }

        var course = await ((CourseRepositoryG)unitOfWork.CourseRepoG)
                                     .GetInculdeDocumentsAsync(id.Value);
        if (course == null)
        {
            return NotFound();
        }

        var courseToReturn = mapper.Map<CourseViewModel>(course);

        return View(courseToReturn);
    }

    // GET: Courses/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Courses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,UploadFiles")] CourseViewModel courseViewModel)
    {
        if (ModelState.IsValid)
        {
            var course = mapper.Map<Course>(courseViewModel);

            await unitOfWork.CourseRepoG
                            .AddAsync(course);

            await UploadFilesAsync(course, courseViewModel.UploadFiles);

            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }


        return View(courseViewModel);
    }


    private async Task UploadFilesAsync(Course? course, IEnumerable<IFormFile>? formFiles)
    {
        IEnumerable<Task<Document>>? documents = formFiles?
                .Select(async formFile
                        => new Document
                        {
                            Name = formFile.FileName,
                            Data = (await formFile.GetBytesAsync()),
                            ContentType = formFile.ContentType,
                            Course = course,
                        });


        if (documents is not null)
        {
            foreach (var document in documents)
            {
                var doc = await document;
                if (doc is not null)
                {
                    await unitOfWork.documentRepo.AddDocument(doc);
                }

            }
        }
    }

    // GET: Courses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await ((CourseRepositoryG)unitOfWork.CourseRepoG)
                                     .GetInculdeDocumentsAsync(id.Value);
        if (course == null)
        {
            return NotFound();
        }

        var courseToReturn = mapper.Map<CourseViewModel>(course);

        return View(courseToReturn);
    }

    // POST: Courses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,UploadFiles")] CourseViewModel courseViewModel)
    {
        if (id != courseViewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var course = mapper.Map<Course>(courseViewModel);

                unitOfWork.CourseRepoG
                          .Update(course);

                await UploadFilesAsync(course, courseViewModel.UploadFiles);

                await unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CourseExists(courseViewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(courseViewModel);
    }

    // GET: Courses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await ((CourseRepositoryG)unitOfWork.CourseRepoG)
                                     .GetInculdeDocumentsAsync(id.Value);

        if (course == null)
        {
            return NotFound();
        }

        var courseToReturn = mapper.Map<CourseViewModel>(course);

        return View(courseToReturn);
    }

    // POST: Courses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await ((CourseRepositoryG)unitOfWork.CourseRepoG)
                                     .GetInculdeDocumentsAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        unitOfWork.CourseRepoG.Delete(course);

        await unitOfWork.CourseRepoG
                        .SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CourseExists(int id)
    {
        return await unitOfWork.CourseRepoG
                               .ExistAsync(id);
    }

    //[Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> Search(string term)
    {
        var courses = await unitOfWork.CourseRepoG.FilterAsync(course => course.Name.Contains(term));
        var coursesToReturn = mapper.Map<IEnumerable<CourseViewModel>>(courses);

        return Json(coursesToReturn);

    }

    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Student_CourseOverview()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (User.IsInRole("Student"))
        {
            var course = await unitOfWork.courseRepo.GetCourseById_IncludeModulesAsync((int)user.CourseId!);

            return View(mapper.Map<CourseOverViewModel>(course));

        }
        return RedirectToPage("LandingActivities");
    }

    [Authorize(Roles = "Student")]
    public async Task<IActionResult> LoadModulePartial(int id)
    {
        var model = await unitOfWork.moduleRepo.GetModulesByCourseId_IncludeActivitiesAsync(id);
        var sorted = model.OrderBy(x => x.StartDate);
        return PartialView("_ModuleView", sorted);
    }
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> LoadStudentPartial(int id)
    {
        var course = await unitOfWork.courseRepo.GetCourseById_IncludeUsersAsync(id);
        var model = course.Users.ToList();
        return PartialView("_StudentView", model);
    }
    //[Authorize(Roles = "Student")]
    public async Task<IActionResult> LoadDocumentsPartial(int id)
    {
        var model = await unitOfWork.documentRepo.GetDocumentsByCourseIdAsync(id);

        return PartialView("_DocumentView", model);
    }


    public async Task<IActionResult> LoadModuleListPartial(int id)
    {
        TempData["cid"] = (int)id;
        TempData.Keep("cid");
        var module = await unitOfWork.moduleRepo.GetModulesByCourseIdAsync(id);
        return PartialView("_ModuleListView", module);
    }
    public async Task<IActionResult> LoadActivityListPartial(int id)
    {
        var activity = await unitOfWork.activityRepo.GetActivitiesByModuleIdAsync(id);

        return PartialView("_ActivityListView", activity);
    }

    public async Task<IActionResult> CalenderTimeLine()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> CourseTimeLine()
    {
        var courses = await unitOfWork.courseRepo.GetAllCourses();
        return Json(data: courses);
    }


    [HttpPost, ActionName("DeleteDocument")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDocument(int id)
    {
        await unitOfWork.documentRepo.DeleteDocument(id);
        await unitOfWork.CompleteAsync();

        return RedirectToAction(nameof(Student_CourseOverview));
    }

}
