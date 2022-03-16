using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace Lms.Web.Areas.Course.Pages.Index;

public class ModuleNavPartialModel : PageModel
{
    private readonly ILogger<ActivitiesModel> logger;
    private readonly ApplicationDbContext db;
    private readonly UserManager<ApplicationUser> userManager;


    public ModuleNavPartialModel(ILogger<ActivitiesModel> logger,
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

    public List<Module> Modules { get; set; } = new List<Module>();

    public async Task<IActionResult> OnGetAsync(int courseId = 1)
    {
        //If the user is signed in, then Continue.
        var appUser = await userManager.GetUserAsync(User);
        if (appUser != null)
        {
            //Logic for when an Admin Accesses this page, then it wouldn't use the Course it's assigned to, but instead a Course from a Selector.
            //if (await userManager.IsInRoleAsync(appUser, "Admin"))
            //{
            //    //This is the Id we need to get from the Selector.
            //    await OnLoadAsync(courseId);
            //}
            //Get the Users Course, which includes it's Modules. 
            await OnLoadAsync(appUser);
            return Page();
        }

        return NotFound();

    }

    private async Task OnLoadAsync(ApplicationUser appUser)
    {
        //Don't know about this query, but Essentially the Course that has a User that matches the user who is logged in.
        var modules = await db.Modules.Where(module => module.CourseId == appUser.CourseId).ToListAsync();

        if (modules == null) throw new NullReferenceException(nameof(modules));
        //Set the list of Modules in the Model to the modules of the Users assigned Course.
        Modules = modules.ToList();
    }
    //Method for if an Admin views the Index he should see the Course selected in the Nav.
    private async Task OnLoadAsync(int courseId)
    {
        //Don't know about this query, but Essentially the Course that has a User that matches the user who is logged in.
        var modules = await db.Modules.Where(module => module.CourseId == courseId).ToListAsync();

        if (modules == null) throw new NullReferenceException(nameof(modules));
        //Set the list of Modules in the Model to the modules of the Users assigned Course.
        Modules = modules.ToList();

    }
}


