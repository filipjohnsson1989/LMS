using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
#nullable disable

namespace Lms.Web.Pages;

public class ActivitiesModel : PageModel
{
    private readonly ILogger<ActivitiesModel> logger;
    private readonly ApplicationDbContext db;
    private readonly UserManager<ApplicationUser> userManager;

    public ActivitiesModel(ILogger<ActivitiesModel> logger,
                                     ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        this.logger = logger ?? throw new NullReferenceException(nameof(logger));
        this.db = db ?? throw new NullReferenceException(nameof(db));
        this.userManager = userManager ?? throw new NullReferenceException(nameof(userManager));
    }
    public string CourseName { get; set; }
    public List<Activity> Activities { get; set; } = new List<Activity>();
    public List<Activity> WeekActivities { get; set; } = new List<Activity>();
    public List<int> Weeks { get; set; } = new List<int>();
    public string NameSort { get; set; }
    public string DateSort { get; set; }
    public string CurrentSort { get; set; }
    public string CurrentFilter { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, string sortOrder, string searchString)
    {
        CurrentSort = sortOrder;
        NameSort = sortOrder == "Name" ? "name_desc" : "Name";
        DateSort = sortOrder == "Date" ? "date_desc" : "Date";
        CurrentFilter = searchString;
        Activities = await db.Activities.Where(a => a.ModuleId == id).OrderBy(a => a.EndDate).Include(a => a.ActivityType).Include(a => a.Documents).ToListAsync();
        if (User.IsInRole("Student"))
        {
           var user = await userManager.GetUserAsync(User);
           var course = db.Courses.Where(c => c.Id == user.CourseId).FirstOrDefault();
           CourseName = course.Name;
        }
        else
        {
            CourseName = "temp";
        }

        //Filter By week. starting from first Monday of the year
        CultureInfo myCI = CultureInfo.GetCultureInfo("sv-SE");
        Calendar myCal = myCI.Calendar;

        var groups = Activities.GroupBy(a => { return myCal.GetWeekOfYear(a.EndDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday); }).Distinct().ToList();
        foreach (var item in groups)
        {
            Weeks.Add(item.Key);
        }




        if (!string.IsNullOrEmpty(searchString))
        {
            Activities = Activities.Where(s => myCal.GetWeekOfYear(s.EndDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday) == int.Parse(CurrentFilter)).ToList();
        }


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

