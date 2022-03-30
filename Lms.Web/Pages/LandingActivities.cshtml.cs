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
    public ApplicationUser CurrentUser { get; set; }
    public string NameSort { get; set; }
    public string DateSort { get; set; }
    public string CurrentSort { get; set; }
    public string CurrentFilter { get; set; }
    public string IsChecked { get; set; }
    public string History { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, string sortOrder, string searchString, string activityFilter, string history)
    {

        string sorting = sortOrder;
        string weekFilter = searchString;
        string activityTypeFilter = activityFilter;
        string expiredActivities = history;

        ////if all inputs are null then reset all saved search and filters
        if (sortOrder == null && searchString == null && activityFilter == null && history == null)
        {
            TempData["WeekFilter"] = null;
            TempData["HistoryFilter"] = History = expiredActivities == "False" ? "True" : "False";
            TempData["ActivityFilter"] = IsChecked = activityTypeFilter == "False" ? "True" : "False";
            TempData["NameSort"] = NameSort = sorting == "Name" ? "name_desc" : "Name";
            TempData["DateSort"] = DateSort = sorting == "Date" ? "date_desc" : "Date";
        }
        if (!string.IsNullOrEmpty(sorting))
        {
            TempData["NameSort"] = NameSort = sorting == "Name" ? "name_desc" : "Name";
            TempData["DateSort"] = DateSort = sorting == "Date" ? "date_desc" : "Date";
        }
        else
        {
            if (TempData["NameSort"] != null)
            {
                NameSort = sorting = TempData.Peek("NameSort").ToString();
                DateSort = sorting = TempData.Peek("NameSort").ToString();
            }

        }
        if (!string.IsNullOrEmpty(activityTypeFilter))
        {
            TempData["ActivityFilter"] = IsChecked = activityTypeFilter == "False" ? "True" : "False";
        }
        else
        {
            if (TempData["ActivityFilter"] != null)
            {
                IsChecked = activityTypeFilter = TempData.Peek("ActivityFilter").ToString();
            }
        }
        if (!string.IsNullOrEmpty(weekFilter))
        {
            TempData["WeekFilter"] = CurrentFilter = weekFilter;
        }
        else
        {
            if (TempData["WeekFilter"] != null)
            {
                CurrentFilter = weekFilter = TempData.Peek("WeekFilter").ToString();
            }
        }
        if (!string.IsNullOrEmpty(expiredActivities))
        {
            TempData["HistoryFilter"] = History = expiredActivities == "False" ? "True" : "False";
        }
        else
        {
            if (TempData["HistoryFilter"] != null)
            {
                History = expiredActivities = TempData.Peek("HistoryFilter").ToString();
            }
        }

        CurrentUser = await userManager.GetUserAsync(User);

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


        if (!string.IsNullOrEmpty(History))
        {
            if (History.Equals("False"))
            {
                Activities = Activities.Where(s => s.EndDate > DateTime.Now).ToList();
            }
        }
        if (!string.IsNullOrEmpty(IsChecked))
        {
            if (IsChecked.Equals("True"))
            {
                Activities = Activities.Where(s => s.ActivityType.Id == 2).ToList();
            }
        }

        //Filter By week. starting from first Monday of the year
        CultureInfo myCI = CultureInfo.GetCultureInfo("sv-SE");
        Calendar myCal = myCI.Calendar;

        var groups = Activities.GroupBy(a => { return myCal.GetWeekOfYear(a.EndDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday); }).Distinct().ToList();
        foreach (var item in groups)
        {
            Weeks.Add(item.Key);
        }




        if (!string.IsNullOrEmpty(CurrentFilter))
        {
            Activities = Activities.Where(s => myCal.GetWeekOfYear(s.EndDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday) == int.Parse(CurrentFilter)).ToList();
        }


        switch (sorting)
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

