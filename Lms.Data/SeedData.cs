using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lms.Data;

public class SeedData
{
    private static RoleManager<IdentityRole> roleManager = default!;
    private static UserManager<ApplicationUser> userManager = default!;
    private static readonly Faker faker = new Faker("sv");
    const int defaultNumberOfInitializingRecords = 8;

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

        await DocumentsInitAsync(context, activities, courses);

        await context.SaveChangesAsync();
    }

    private static async Task DocumentsInitAsync(ApplicationDbContext context, IEnumerable<Activity> activities, IEnumerable<Course> courses)
    {
        string Name = "dummyDoc";
        var docs = new List<Document>();
        var data = new Byte[50];
        for (int i = 0; i < 50; i++)
        {
            data[i] = faker.System.Random.Byte();
        }
        foreach (var course in courses)
        {
            for (int i = 0; i < faker.Random.Number(1, 5); i++)
            {
                var doc = new Document()
                {
                    Name = Name,
                    Course = course,
                    Data = data,
                    ContentType = "Dummy"
                };
                docs.Add(doc);
            }
        }

        foreach (var activity in activities)
        {
            var doc = new Document()
            {
                Name = Name,
                Activity = activity,
                Data = data,
                ContentType = "Dummy"
            };
            docs.Add(doc);
        }
        await context.AddRangeAsync(docs);
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
                Description = faker.Lorem.Sentences(5),
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
            var numBuffer = 1;
            for (int i = 0; i < faker.Random.Number(8, 16); i++)
            {
                var startDate = faker.Date.Between(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(10));
                var temp = new Module
                {
                    Name = faker.Company.CatchPhrase(),
                    Description = faker.Lorem.Sentences(3),
                    StartDate = startDate,
                    EndDate = faker.Date.Between(startDate.AddDays(numBuffer), startDate.AddDays(3)),
                    Course = course,
                };
                numBuffer += 3;
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
                for (int i = 0; i < faker.Random.Number(8, 16); i++)
                {
                    var startDate = faker.Date.Between(module.StartDate, module.EndDate);
                    var randomDuration = faker.Random.Number(1, 48);

                    var temp = new Activity
                    {
                        Name = faker.Company.CatchPhrase(),
                        Description = faker.Lorem.Sentences(2),
                        StartDate = startDate,
                        EndDate = faker.Date.Between(startDate.AddDays(i), startDate.AddDays(i).AddHours(randomDuration)),
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
