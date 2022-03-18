using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace Lms.Web.Pages.Module;

public class ActivitiesModel : PageModel
{
    private readonly ILogger<ActivitiesModel> logger;
    private readonly ApplicationDbContext db;

    public ActivitiesModel(ILogger<ActivitiesModel> logger,
                                     ApplicationDbContext db)
    {
        if (logger == null) throw new NullReferenceException(nameof(logger));
        if (db == null) throw new NullReferenceException(nameof(db));

        this.logger = logger;
        this.db = db;

    }
    public List<Activity> Activities { get; set; } = new List<Activity>();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Activities = await db.Activities.Where(a => a.ModuleId == id).OrderBy(a => a.EndDate).Include(a => a.ActivityType).ToListAsync();
        return Page();
    }

}

