using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Lms.Core.Dtos.Course;

namespace Lms.Web.Controllers;

public class CoursesController : Controller
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<ApplicationUser> userManager;

    private readonly IMapper mapper;

    public CoursesController(IUnitOfWork unitOfWork,
                             UserManager<ApplicationUser> userManager,
                             IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    // GET: Courses
    public async Task<IActionResult> Index()
    {
        var courses = await unitOfWork.CourseRepoG
                                    .GetAllAsync();
        var coursesToReturn = mapper.Map<IEnumerable<CourseDto>>(courses);
        return View(coursesToReturn);
    }

    // GET: Courses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await unitOfWork.CourseRepoG
                                     .GetAsync(id.Value);
        if (course == null)
        {
            return NotFound();
        }

        var courseToReturn = mapper.Map<CourseDto>(course);

        return View(courseToReturn);
    }

    // GET: Courses/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Courses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate")] CourseDto courseDto)
    {
        if (ModelState.IsValid)
        {
            var course = mapper.Map<Course>(courseDto);

            await unitOfWork.CourseRepoG
                            .AddAsync(course);

            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        var courseToReturn = mapper.Map<CourseDto>(courseDto);

        return View(courseToReturn);
    }

    // GET: Courses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await unitOfWork.CourseRepoG
                                     .GetAsync(id.Value);
        if (course == null)
        {
            return NotFound();
        }

        var courseToReturn = mapper.Map<CourseDto>(course);

        return View(courseToReturn);
    }

    // POST: Courses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate")] CourseDto courseDto)
    {
        if (id != courseDto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var course = mapper.Map<Course>(courseDto);

                unitOfWork.CourseRepoG
                          .Update(course);
                await unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CourseExists(courseDto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(courseDto);
    }

    // GET: Courses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await unitOfWork.CourseRepoG
                                     .GetAsync(id.Value);

        if (course == null)
        {
            return NotFound();
        }

        var courseToReturn = mapper.Map<CourseDto>(course);

        return View(courseToReturn);
    }

    // POST: Courses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await unitOfWork.CourseRepoG
                                     .GetAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        unitOfWork.CourseRepoG.Delete(course);

        await unitOfWork.CourseRepoG
                        .SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CourseExists(int id)
    {
        return await unitOfWork.CourseRepoG
                               .ExistAsync(id);
    }

    //[Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> Search(string term)
    {
        var courses = await unitOfWork.CourseRepoG.FindAsync(course => course.Name.Contains(term));
        var coursesToReturn = mapper.Map<IEnumerable<CourseDto>>(courses);

        return Json(coursesToReturn);

    }
}
