using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Lms.Core.ViewModels.Modules;
using Lms.Web.Extensions;
using Lms.Core.ViewModels.Courses;

namespace Lms.Web.Controllers;

public class ModulesController : Controller
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<ApplicationUser> userManager;

    private readonly IMapper mapper;

    public ModulesController(IUnitOfWork unitOfWork,
                             UserManager<ApplicationUser> userManager,
                             IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    // GET: Modules
    public async Task<IActionResult> Index(int? courseId)
    {
        var modules = await unitOfWork.ModuleRepoG
                                      .GetAllAsync(courseId);

        var modulesToReturn = mapper.Map<IEnumerable<ModuleViewModel>>(modules);

        string? courseName = default!;
        if (courseId is not null)
        {
            var course = await unitOfWork.CourseRepoG.GetAsync(courseId.Value);
            courseName = course?.Name;
        }

        IndexModuleViewModel modelToReturn = new() { CourseId = courseId, CourseName = courseName, Modules = modulesToReturn };

        return View(modelToReturn);
    }

    // GET: Modules/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var module = await unitOfWork.ModuleRepoG
                                     .GetAsync(id.Value);
        if (module == null)
        {
            return NotFound();
        }

        var moduleToReturn = mapper.Map<ModuleViewModel>(module);

        return View(moduleToReturn);
    }

    // GET: Modules/Create
    public async Task<IActionResult> Create(int? courseId)
    {
        if (courseId is null)
            return View();

        var course = await unitOfWork.CourseRepoG
            .GetAsync(courseId.Value);

        var courseModel = mapper.Map<SearchCourseViewModel>(course)!;

        CreateEditModuleViewModel modelToReturn = new() { Course = courseModel };
        TempData["CourseIdForCreateModule"] = courseId.Value;
        return View(modelToReturn);


    }

    // POST: Modules/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,Course,UploadFiles")] CreateEditModuleViewModel moduleViewModel)
    {
        if (ModelState.IsValid)
        {
            var module = mapper.Map<Module>(moduleViewModel);

            var course = await unitOfWork.CourseRepoG.GetAsync(moduleViewModel.Course.Id);
            if (course is null)
            {
                return BadRequest();
            }
            module.Course = course;

            await unitOfWork.ModuleRepoG
                            .AddAsync(module);

            await UploadFilesAsync(module, moduleViewModel.UploadFiles);


            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }


        return View(moduleViewModel);
    }

    private async Task UploadFilesAsync(Module? module, IEnumerable<IFormFile>? formFiles)
    {
        IEnumerable<Task<Document>>? documents = formFiles?
                .Select(async formFile
                        => new Document
                        {
                            Name = formFile.FileName,
                            Data = (await formFile.GetBytesAsync()),
                            ContentType = formFile.ContentType,
                            Module = module,
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

    // GET: Modules/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var module = await unitOfWork.ModuleRepoG
                                     .GetAsync(id.Value);
        if (module == null)
        {
            return NotFound();
        }

        var moduleToReturn = mapper.Map<CreateEditModuleViewModel>(module);

        return View(moduleToReturn);
    }

    // POST: Modules/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,Course,UploadFiles")] CreateEditModuleViewModel moduleViewModel)
    {
        if (id != moduleViewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var module = mapper.Map<Module>(moduleViewModel);

                var course = await unitOfWork.CourseRepoG.GetAsync(moduleViewModel.Course.Id);
                if (course is null)
                {
                    return BadRequest();
                }
                module.Course = course;

                unitOfWork.ModuleRepoG
                          .Update(module);

                await UploadFilesAsync(module, moduleViewModel.UploadFiles);

                await unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ModuleExists(moduleViewModel.Id))
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
        return View(moduleViewModel);
    }

    // GET: Modules/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var module = await unitOfWork.ModuleRepoG
                                     .GetAsync(id.Value);

        if (module == null)
        {
            return NotFound();
        }

        var moduleToReturn = mapper.Map<ModuleViewModel>(module);

        return View(moduleToReturn);
    }

    // POST: Modules/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var module = await unitOfWork.ModuleRepoG
                                     .GetAsync(id);
        if (module == null)
        {
            return NotFound();
        }

        unitOfWork.ModuleRepoG.Delete(module);

        await unitOfWork.ModuleRepoG
                        .SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ModuleExists(int id)
    {
        return await unitOfWork.ModuleRepoG
                               .ExistAsync(id);
    }

    //[Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> Search(string term)
    {
        var modules = await unitOfWork.ModuleRepoG.FilterAsync(course => course.Name.Contains(term));
        var coursesToReturn = mapper.Map<IEnumerable<ModuleViewModel>>(modules);

        return Json(coursesToReturn);

    }

    public async Task<IActionResult> CalenderTimeLine()
    {
        return View();
    }


    [HttpGet]
    [Route("Modules/ModuleTimeLine/")]
    public async Task<IActionResult> ModuleTimeLine()
    {
        int courseId = (int)TempData["CourseIdForCreateModule"];
        var modules = await unitOfWork.moduleRepo.GetModulesByCourseIdAsync(courseId);
        return Json(data: modules);
    }
    public async Task<IActionResult> VerifyStartdate(DateTime StartDate, int CourseId)
    {
        //Check if startdate already exists in the database. Sends a warning if it exists

        var moduels = await unitOfWork.ModuleRepoG.FilterAsync(module => module.EndDate > StartDate && module.CourseId == CourseId);
        var module = moduels.FirstOrDefault();

        if (module != null)
        {
            return Json($"Enter a valid Start Date.");
        }
        return Json(true);
    }
    public IActionResult VerifyEnddate(DateTime EndDate, DateTime StartDate)
    {
        //Check if startdate already exists in the database. Sends a warning if it exists

        if (EndDate < StartDate)
        {
            return Json($"Enter a valid End Date.");
        }
        return Json(true);
    }
}
