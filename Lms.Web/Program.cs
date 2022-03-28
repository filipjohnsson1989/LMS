using Lms.Data;
using Lms.Data.Repositories;
using Lms.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Lms.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Lms.Data.AutoMapper;
using Lms.Web.Services;
using Lms.Web.Conventions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IRepository<Course>, CourseRepositoryG>();
builder.Services.AddTransient<IRepository<ActivityType>, ActivityTypeRepository>();
//builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.Add(new GlobalTemplatePageRouteModelConvention());

});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICourseSelector, CourseSelector>();
builder.Services.AddAutoMapper(typeof(LMSMappings));


builder.Services.AddAutoMapper(typeof(CourseProfile));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    var config = services.GetRequiredService<IConfiguration>();

    //db.Database.EnsureDeleted();
    //db.Database.Migrate();

    //dotnet user-secrets set "AdminPW" "L�seord1!"
    var adminPW = config["AdminPW"];

    try
    {
        SeedData.InitAsync(db, services, adminPW).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        throw;
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Courses}/{action=Add_Course_Module_Activity}/{id?}");
    pattern: "{controller=Courses}/{action=Add_Course_Module_Activity}/{id?}");
app.MapRazorPages();

app.Run();
