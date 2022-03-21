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
    public string NameSort { get; set; }
    public string DateSort { get; set; }
    public string CurrentSort { get; set; }


    public async Task<IActionResult> OnGetAsync(int id, string sortOrder)
    {
        CurrentSort = sortOrder;
        NameSort = sortOrder == "Name" ? "name_desc" : "Name";
        DateSort = sortOrder == "Date" ? "date_desc" : "Date";


        Activities = await db.Activities.Where(a => a.ModuleId == id).OrderBy(a => a.EndDate).Include(a => a.ActivityType).Include(a => a.Documents).ToListAsync();


        switch (sortOrder)
        {
            case "name_desc":
                Activities = Activities.OrderByDescending(s => s.Name).ToList();
                break;
            case "Name":
                Activities = Activities.OrderBy(s => s.Name).ToList();
                break;
            case "date_desc":
                Activities = Activities.OrderByDescending(s => s.EndDate).ToList();
                break;

            case "Date":
                Activities = Activities.OrderBy(s => s.EndDate).ToList();
                break;
            default:
                Activities = Activities.OrderBy(s => s.EndDate).ToList();
                break;
        }
        return Page();
    }

}

