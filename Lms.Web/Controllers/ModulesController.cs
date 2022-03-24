using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Lms.Core.ViewModels.Modules;

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
    public async Task<IActionResult> Index()
    {
        var modules = await unitOfWork.ModuleRepoG
                                      .GetAllAsync();

        var modulesToReturn = mapper.Map<IEnumerable<ModuleViewModel>>(modules);
        return View(modulesToReturn);
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
    public IActionResult Create()
    {
        return View();
    }

    // POST: Modules/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,Course")] CreateEditModuleViewModel moduleViewModel)
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

            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        var moduleToReturn = mapper.Map<CreateEditModuleViewModel>(moduleViewModel);

        return View(moduleToReturn);
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
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,Course")] CreateEditModuleViewModel moduleViewModel)
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
        var modules = await unitOfWork.ModuleRepoG.FindAsync(course => course.Name.Contains(term));
        var coursesToReturn = mapper.Map<IEnumerable<ModuleViewModel>>(modules);

        return Json(coursesToReturn);

    }
}
