using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace Lms.Web.Areas.Course.Pages.Index;

public class ActivitiesModel : PageModel
{
    private readonly ILogger<ActivitiesModel> logger;
    private readonly ApplicationDbContext db;
    private readonly UserManager<ApplicationUser> userManager;

    public ActivitiesModel(ILogger<ActivitiesModel> logger,
                                     ApplicationDbContext db,
                                     UserManager<ApplicationUser> userManager)
    {
        if (logger == null) throw new NullReferenceException(nameof(logger));
        if (db == null) throw new NullReferenceException(nameof(db));
        if (userManager == null) throw new NullReferenceException(nameof(userManager));
        this.logger = logger;
        this.db = db;
        this.userManager = userManager;
    }
    [Display(Name = "Title")]
    public string Name { get; set; }
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    public List<Activity> Activities { get; set; } = new List<Activity>();

    public async Task OnGetAsync(int id)
    {
        Activities = await db.Activities.Where(a => a.ModuleId == id).ToListAsync();
    }

}

