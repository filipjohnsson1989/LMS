using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Lms.Core.Dtos.Activities;

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

        var activitiesToReturn = mapper.Map<IEnumerable<ActivityDto>>(activities);
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

        var activityToReturn = mapper.Map<ActivityDto>(activity);

        return View(activityToReturn);
    }

    // GET: Activities/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Activities/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,Module")] CreateEditActivityDto activityDto)
    {
        if (ModelState.IsValid)
        {
            var activity = mapper.Map<Activity>(activityDto);

            var module = await unitOfWork.ModuleRepoG.GetAsync(activityDto.Module.Id);
            if (module is null)
            {
                return BadRequest();
            }
            activity.Module = module;

            await unitOfWork.ActivityRepoG
                            .AddAsync(activity);

            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        var activityToReturn = mapper.Map<CreateEditActivityDto>(activityDto);

        return View(activityToReturn);
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

        var activityToReturn = mapper.Map<CreateEditActivityDto>(activity);

        return View(activityToReturn);
    }

    // POST: Activities/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,Module")] CreateEditActivityDto activityDto)
    {
        if (id != activityDto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var activity = mapper.Map<Activity>(activityDto);

                var module = await unitOfWork.ModuleRepoG.GetAsync(activityDto.Module.Id);
                if (module is null)
                {
                    return BadRequest();
                }
                activity.Module = module;

                unitOfWork.ActivityRepoG
                          .Update(activity);
                await unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ActivityExists(activityDto.Id))
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
        return View(activityDto);
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

        var activityToReturn = mapper.Map<ActivityDto>(activity);

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
