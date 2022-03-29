﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Lms.Core.ViewModels.Modules;
using Microsoft.AspNetCore.Authorization;

namespace Lms.Web.Controllers;

public class ModulesController : Controller
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<ApplicationUser> userManager;

    private readonly IMapper mapper;
    private readonly ApplicationDbContext db;

    public ModulesController(IUnitOfWork unitOfWork,
                             UserManager<ApplicationUser> userManager,
                             IMapper mapper, ApplicationDbContext db)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
        this.mapper = mapper;
        this.db = db;
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
    public IActionResult CreateModule(int courseid)
    {

        TempData["courseid"] = courseid;
        return View();
    }

    // POST: Modules/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateModule(CreateModuleModel module)
    {
        var moduleobj = mapper.Map<Module>(module);

        //var mobj = db.Modules.Where(d => d.EndDate > moduleobj.StartDate || moduleobj.EndDate<moduleobj.StartDate).FirstOrDefault(c => c.CourseId == moduleobj.CourseId);
        //if (mobj != null)
        //{
        //    return Json($"Enter a Valid Date");
        //}

        if (ModelState.IsValid )
        {
            await unitOfWork.moduleRepo.AddModule(moduleobj);
            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(moduleobj);
    }
    //public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,Course")] CreateEditModuleViewModel moduleViewModel)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var module = mapper.Map<Module>(moduleViewModel);

    //        var course = await unitOfWork.CourseRepoG.GetAsync(moduleViewModel.Course.Id);
    //        if (course is null)
    //        {
    //            return BadRequest();
    //        }
    //        module.Course = course;

    //        await unitOfWork.ModuleRepoG
    //                        .AddAsync(module);

    //        await unitOfWork.CompleteAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    var moduleToReturn = mapper.Map<CreateEditModuleViewModel>(moduleViewModel);

    //    return View(moduleToReturn);
    //}

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
    public async Task<IActionResult> CalenderTimeLine()
    {
        return View();
    }
    [HttpGet]
    [Route("Modules/ModuleTimeLine/")]
    public async Task<IActionResult> ModuleTimeLine()
    {
        int x = (int)TempData["cid"];
        var modules = await unitOfWork.moduleRepo.GetModulesByCourseIdAsync(x);
            return Json(data: modules);
    }
    //[AcceptVerbs("GET,POST")]
    //[AllowAnonymous]
    public IActionResult VerifyStartdate(DateTime StartDate, int CourseId)
    {
        //Check if startdate already exists in the database. Sends a warning if it exists

        var moduleobj = db.Modules.Where(d => d.EndDate > StartDate).FirstOrDefault(c => c.CourseId == CourseId);
        if (moduleobj != null)
        {
            return Json($"Enter a valid Start Date.");
        }
        return Json(true);
    }
    //[AllowAnonymous]
    public IActionResult VerifyEnddate(DateTime EndDate, DateTime StartDate)
    {
        //Check if startdate already exists in the database. Sends a warning if it exists

       if(EndDate < StartDate)
        {
            return Json($"Enter a valid End Date.");
        }
        return Json(true);
    }

}
