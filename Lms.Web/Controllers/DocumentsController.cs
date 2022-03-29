using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data;
using AutoMapper;

namespace Lms.Web.Controllers;

public class DocumentsController : Controller
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<ApplicationUser> userManager;

    private readonly IMapper mapper;

    public DocumentsController(IUnitOfWork unitOfWork,
                             UserManager<ApplicationUser> userManager,
                             IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    // GET: Documents
    public async Task<IActionResult> Index()
    {
        return View(await unitOfWork.DocumentRepoG.GetAllAsync());
    }

    // GET: Documents/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var document = await unitOfWork.DocumentRepoG.GetAsync(id.Value);
        if (document == null)
        {
            return NotFound();
        }

        return View(document);
    }

    // GET: Documents/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Documents/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Data,ContentType,UploadDate,Id")] Document document)
    {
        if (ModelState.IsValid)
        {
            await unitOfWork.DocumentRepoG.AddAsync(document);
            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(document);
    }

    // GET: Documents/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var document = await unitOfWork.DocumentRepoG.GetAsync(id.Value);
        if (document == null)
        {
            return NotFound();
        }
        return View(document);
    }

    // POST: Documents/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Name,Data,ContentType,UploadDate,Id")] Document document)
    {
        if (id != document.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                unitOfWork.DocumentRepoG.Update(document);
                await unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DocumentExistsAsync(document.Id))
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
        return View(document);
    }

    // GET: Documents/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var document = await unitOfWork.DocumentRepoG.GetAsync(id.Value);
        if (document == null)
        {
            return NotFound();
        }

        return View(document);
    }

    // POST: Documents/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var document = await unitOfWork.DocumentRepoG.GetAsync(id);
        if (document == null)
        {
            return NotFound();
        }
        unitOfWork.DocumentRepoG.Delete(document);
        await unitOfWork.CompleteAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> DocumentExistsAsync(int id)
    {
        return await unitOfWork.DocumentRepoG.ExistAsync(id);
    }

    public async Task<ActionResult> DownloadAsync(int id)
    {
        var document = await unitOfWork.DocumentRepoG.GetAsync(id);
        if (document == null)
        {
            return NotFound();
        }

        return File(document.Data, document.ContentType);
    }
}
