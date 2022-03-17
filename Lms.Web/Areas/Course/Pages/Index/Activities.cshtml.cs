using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace Lms.Web.Areas.Course.Pages.Index;

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
    [Display(Name = "Title")]
    public string Name { get; set; }
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    public List<Activity> Activities { get; set; } = new List<Activity>();

    public void OnGet(int id)
    {
        Activities = db.Activities.Where(a => a.ModuleId == id).ToList();
    }

}

