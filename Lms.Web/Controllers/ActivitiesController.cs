using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Lms.Core.ViewModels.Activities;
using Lms.Web.Extensions;
using Lms.Core.ViewModels.Modules;

namespace Lms.Web.Controllers;

public class ActivitiesController : Controller
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<ApplicationUser> userManager;

    private readonly IMapper mapper;

    public ActivitiesController(IUnitOfWork unitOfWork,
                             UserManager<ApplicationUser> userManager,
                             IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    // GET: Activities
    public async Task<IActionResult> Index()
    {
        var activities = await unitOfWork.ActivityRepoG
                                      .GetAllAsync();

        var activitiesToReturn = mapper.Map<IEnumerable<ActivityViewModel>>(activities);
        return View(activitiesToReturn);
    }

    // GET: Activities/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var activity = await unitOfWork.ActivityRepoG
                                     .GetAsync(id.Value);
        if (activity == null)
        {
            return NotFound();
        }

        var activityToReturn = mapper.Map<ActivityViewModel>(activity);

        return View(activityToReturn);
    }

    // GET: Activities/Create
    public async Task<IActionResult> Create(int? moduleId)
    {
        if (moduleId is null)
            return View();

        var module = await unitOfWork.ActivityRepoG
            .GetAsync(moduleId.Value);

        var moduleModel = mapper.Map<SearchModuleViewModel>(module)!;

        CreateEditActivityViewModel modelToReturn = new() { Module = moduleModel };
        TempData["ModuleIdForCreateActivity"] = moduleId.Value;
        return View(modelToReturn);
    }

    // POST: Activities/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,Module,ActivityType,UploadFiles")] CreateEditActivityViewModel activityViewModel)
    {
        if (ModelState.IsValid)
        {
            var activity = mapper.Map<Activity>(activityViewModel);

            var module = await unitOfWork.ModuleRepoG.GetAsync(activityViewModel.Module.Id);
            if (module is null)
            {
                return BadRequest();
            }
            activity.Module = module;

            var activityType = await unitOfWork.ActivityTypeRepo.GetAsync(activityViewModel.ActivityType.Id);
            if (activityType is null)
            {
                return BadRequest();
            }
            activity.ActivityType = activityType;

            await unitOfWork.ActivityRepoG
                            .AddAsync(activity);

            await UploadFilesAsync(activity, activityViewModel.UploadFiles);

            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }


        return View(activityViewModel);
    }

    private async Task UploadFilesAsync(Activity? activity, IEnumerable<IFormFile>? formFiles)
    {
        IEnumerable<Task<Document>>? documents = formFiles?
                .Select(async formFile
                        => new Document
                        {
                            Name = formFile.FileName,
                            Data = (await formFile.GetBytesAsync()),
                            ContentType = formFile.ContentType,
                            Activity = activity,
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


    // GET: Activities/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var activity = await unitOfWork.ActivityRepoG
                                     .GetAsync(id.Value);
        if (activity == null)
        {
            return NotFound();
        }

        var activityToReturn = mapper.Map<CreateEditActivityViewModel>(activity);

        return View(activityToReturn);
    }

    // POST: Activities/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,Module,ActivityType,UploadFiles")] CreateEditActivityViewModel activityViewModel)
    {
        if (id != activityViewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var activity = mapper.Map<Activity>(activityViewModel);

                var module = await unitOfWork.ModuleRepoG.GetAsync(activityViewModel.Module.Id);
                if (module is null)
                {
                    return BadRequest();
                }
                activity.Module = module;

                var activityType = await unitOfWork.ActivityTypeRepo.GetAsync(activityViewModel.ActivityType.Id);
                if (activityType is null)
                {
                    return BadRequest();
                }
                activity.ActivityType = activityType;

                unitOfWork.ActivityRepoG
                          .Update(activity);

                await UploadFilesAsync(activity, activityViewModel.UploadFiles);

                await unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ActivityExists(activityViewModel.Id))
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
        return View(activityViewModel);
    }

    // GET: Activities/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var activity = await unitOfWork.ActivityRepoG
                                     .GetAsync(id.Value);

        if (activity == null)
        {
            return NotFound();
        }

        var activityToReturn = mapper.Map<ActivityViewModel>(activity);

        return View(activityToReturn);
    }

    // POST: Activities/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var activity = await unitOfWork.ActivityRepoG
                                     .GetAsync(id);
        if (activity == null)
        {
            return NotFound();
        }

        unitOfWork.ActivityRepoG.Delete(activity);

        await unitOfWork.ActivityRepoG
                        .SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ActivityExists(int id)
    {
        return await unitOfWork.ActivityRepoG
                               .ExistAsync(id);
    }
}
