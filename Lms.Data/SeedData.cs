using Bogus;
using Lms.Core.Entities;
using Lms.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Lms.Data;

public class SeedData
{
    private static RoleManager<IdentityRole> roleManager = default!;
    private static UserManager<ApplicationUser> userManager = default!;
    private static readonly Faker faker = new Faker("sv");
    const int defaultNumberOfInitializingRecords = 20;

    public static async Task InitAsync(ApplicationDbContext context, IServiceProvider services, string adminPW)
    {
        if (string.IsNullOrWhiteSpace(adminPW)) throw new Exception("Cant get password from config");
        if (context is null) throw new NullReferenceException(nameof(ApplicationDbContext));

        roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        if (roleManager is null) throw new NullReferenceException(nameof(RoleManager<IdentityRole>));

        userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        if (userManager is null) throw new NullReferenceException(nameof(UserManager<ApplicationUser>));

        await RoleInitAsync(context);

        await AdminInitAsync(context, services, adminPW);

        var courses = await CourseInitAsync(context);

        await StudentInitAsync(context, courses);

        var modules = await ModuleInitAsync(context, courses);

        var activityTypes = await ActivityTypeInitAsync(context);

        var activities = await ActivityInitAsync(context, activityTypes, modules);

        await DocumentsInitAsync(context, activities);

        await context.SaveChangesAsync();
    }

    private static Task DocumentsInitAsync(ApplicationDbContext context, IEnumerable<Activity> activities)
    {
        string 
    }

    private static async Task RoleInitAsync(ApplicationDbContext db)
    {
        if (await db.Roles.AnyAsync()) return;

        var roleNames = new[] { "Teacher", "Student" };

        await AddRolesAsync(roleNames);


    }

    private static async Task AdminInitAsync(ApplicationDbContext db, IServiceProvider services, string adminPW)
    {
        var roleNames = new[] { "Teacher" };

        var teacherRole = await db.Roles.Where(role => role.Name == roleNames[0]).FirstOrDefaultAsync();
        if (teacherRole is null) return;

        if (await db.UserRoles.Where(userRole => userRole.RoleId == teacherRole.Id).AnyAsync()) return;


        string adminEmail = "admin@lms.se";
        var admin = await AddUserAsync(adminEmail, adminPW);
        await AddToRolesAsync(admin, roleNames);

    }

    private static async Task AddRolesAsync(string[] roleNames)
    {
        foreach (var roleName in roleNames)
        {
            if (await roleManager.RoleExistsAsync(roleName)) continue;
            var role = new IdentityRole { Name = roleName };
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }

    private static async Task<ApplicationUser> AddUserAsync(string email, string? password)
    {
        var found = await userManager.FindByEmailAsync(email);

        if (found != null) return null!;

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            //FirstName = "User/Admin/Teacher/Student",
            //TimeOfRegistration = DateTime.Now
        };

        var result = await userManager.CreateAsync(user, password ?? email);
        if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

        return user;
    }

    private static async Task AddToRolesAsync(ApplicationUser user, string[] roleNames)
    {
        if (user is null) throw new NullReferenceException(nameof(user));

        foreach (var role in roleNames)
        {
            if (await userManager.IsInRoleAsync(user, role)) continue;
            var result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }



    private static async Task<IEnumerable<Course>> CourseInitAsync(ApplicationDbContext db)
    {
        IEnumerable<Course> courses = new List<Course>();

        if (await db.Courses.AnyAsync()) return courses;

        courses = GetCourses();
        await db.AddRangeAsync(courses);

        return courses;

    }


    private static IEnumerable<Course> GetCourses()
    {
        var courses = new List<Course>();

        for (int i = 0; i < defaultNumberOfInitializingRecords; i++)
        {
            var temp = new Course
            {
                Name = faker.Company.CatchPhrase(),
                Description = faker.Hacker.Verb(),
                StartDate = DateTime.Now.AddDays(faker.Random.Int(-10, 10))
            };

            courses.Add(temp);
        }
        return courses;
    }

    private static async Task StudentInitAsync(ApplicationDbContext db, IEnumerable<Course> courses)
    {
        var roleNames = new[] { "Student" };

        var studentRole = await db.Roles.Where(role => role.Name == roleNames[0]).FirstOrDefaultAsync();
        if (studentRole is null) return;

        if (await db.UserRoles.Where(userRole => userRole.RoleId == studentRole.Id).AnyAsync()) return;

        if (!courses.Any()) courses = await db.Courses.ToListAsync();

        const int numberOfStudentAtCourse = 15;
        const int numberOfInitializingUsers = defaultNumberOfInitializingRecords * numberOfStudentAtCourse;
        for (int i = 0; i < numberOfInitializingUsers; i++)
        {

            var student = await AddUserAsync($"Student{i}@lms.se", null);

            var courseIndex = i % defaultNumberOfInitializingRecords;
            student.Course = courses.ElementAt(courseIndex);

            await AddToRolesAsync(student, roleNames);
        }

    }




    private static async Task<IEnumerable<Module>> ModuleInitAsync(ApplicationDbContext db, IEnumerable<Course> courses)
    {
        IEnumerable<Module> modules = new List<Module>();

        if (await db.Modules.AnyAsync()) return modules;
        if (!courses.Any()) courses = await db.Courses.ToListAsync();

        modules = GetModules(db, courses);
        await db.AddRangeAsync(modules);

        return modules;

    }

    private static IEnumerable<Module> GetModules(ApplicationDbContext db, IEnumerable<Course> courses)
    {
        var modules = new List<Module>();

        foreach (var course in courses)
        {
            for (int i = 0; i < defaultNumberOfInitializingRecords; i++)
            {
                var startDate = faker.Date.Between(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(10));
                var temp = new Module
                {
                    Name = faker.Company.CatchPhrase(),
                    Description = faker.Hacker.Verb(),
                    StartDate = startDate,
                    EndDate = faker.Date.Between(startDate.AddDays(30), startDate.AddDays(90)),
                    Course = course,
                };

                modules.Add(temp);
            }
        }

        return modules;
    }

    private static async Task<IEnumerable<ActivityType>> ActivityTypeInitAsync(ApplicationDbContext db)
    {
        IEnumerable<ActivityType> activityTypes = new List<ActivityType>();

        if (await db.ActivityTypes.AnyAsync()) return activityTypes;

        activityTypes = GetActivityTypes(db);
        await db.AddRangeAsync(activityTypes);

        return activityTypes;

    }

    private static IEnumerable<ActivityType> GetActivityTypes(ApplicationDbContext db)
    {
        var activityTypes = new List<ActivityType>();

        var lecture = new ActivityType() { Name = "Lecture" };
        activityTypes.Add(lecture);

        var assignment = new ActivityType() { Name = "Assignment" };
        activityTypes.Add(assignment);

        activityTypes.AddRange(activityTypes);

        return activityTypes;
    }

    private static async Task<IEnumerable<Activity>> ActivityInitAsync(ApplicationDbContext db, IEnumerable<ActivityType> activityTypes, IEnumerable<Module> modules)
    {
        IEnumerable<Activity> activities = new List<Activity>();

        if (await db.Activities.AnyAsync()) return activities;
        if (!modules.Any()) modules = await db.Modules.ToListAsync();

        activities = GetActivities(db, activityTypes, modules);
        await db.AddRangeAsync(activities);

        return activities;

    }

    private static IEnumerable<Activity> GetActivities(ApplicationDbContext db, IEnumerable<ActivityType> activityTypes, IEnumerable<Module> modules)
    {
        var activities = new List<Activity>();

        foreach (var module in modules)
        {
            foreach (var activityType in activityTypes)
            {
                for (int i = 0; i < defaultNumberOfInitializingRecords; i++)
                {
                    var startDate = faker.Date.Between(module.StartDate, module.EndDate);
                    var temp = new Activity
                    {
                        Name = faker.Company.CatchPhrase(),
                        Description = faker.Hacker.Verb(),
                        StartDate = startDate,
                        EndDate = faker.Date.Between(startDate.AddDays(30), startDate.AddDays(90)),
                        ActivityType = activityType,
                        Module = module,
                    };

                    activities.Add(temp);
                }
            }
        }

        return activities;
    }
}
