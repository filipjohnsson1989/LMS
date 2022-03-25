using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Controllers;

public class ActivityTypesController : Controller
{
    private readonly IUnitOfWork unitOfWork;

    public ActivityTypesController(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

    // GET: ActivityTypes
    public async Task<IActionResult> Index()
    {
        var activityTypes = await unitOfWork.ActivityTypeRepo
            .GetAllAsync();
        var docs = await unitOfWork.documentRepo
            .GetAllDocuments();
        UpDownLoadCombinedTestViewModel testModel = new UpDownLoadCombinedTestViewModel();
        testModel.Documents = docs;
        testModel.ActivityTypes = activityTypes;
        return View(testModel);

    }

    // GET: ActivityTypes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var activityType = await unitOfWork.ActivityTypeRepo
            .GetAsync(id.Value);
        if (activityType == null)
        {
            return NotFound();
        }

        return View(activityType);
    }

    // GET: ActivityTypes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ActivityTypes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] ActivityType activityType)
    {
        if (ModelState.IsValid)
        {
            await unitOfWork.ActivityTypeRepo
                .AddAsync(activityType);
            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(activityType);
    }

    // GET: ActivityTypes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var activityType = await unitOfWork.ActivityTypeRepo
            .GetAsync(id.Value);
        if (activityType == null)
        {
            return NotFound();
        }
        return View(activityType);
    }

    // POST: ActivityTypes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ActivityType activityType)
    {
        if (id != activityType.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                unitOfWork.ActivityTypeRepo.Update(activityType);
                await unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await unitOfWork.ActivityTypeRepo.ExistAsync(activityType.Id))
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
        return View(activityType);
    }

    // GET: ActivityTypes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var activityType = await unitOfWork.ActivityTypeRepo
            .GetAsync(id.Value);
        if (activityType == null)
        {
            return NotFound();
        }

        return View(activityType);
    }

    // POST: ActivityTypes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var activityType = await unitOfWork.ActivityTypeRepo
            .GetAsync(id);
        if (activityType == null)
        {
            return NotFound();
        }
        unitOfWork.ActivityTypeRepo
            .Delete(activityType);
        await unitOfWork.ActivityTypeRepo
            .SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<FileResult> DownloadAsync(int id)
    {
        var fileToRetrieve = await unitOfWork.documentRepo.GetDocumentById(id);
        return File(fileToRetrieve.Data, fileToRetrieve.ContentType);
    }
    [HttpPost]
    public async Task<IActionResult> FormUpload(UploadFileViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Upload != null)
            {
                var file = model.Upload;
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    var fileExtension = Path.GetExtension(fileName);
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);
                    var contentType = file.ContentType;
                    var course = model.Course;
                    var module = model.Module;
                    var activity = model.Activity;
                    var applicationUser = model.User;

                    var objfiles = new Document()
                    {
                        Name = newFileName,
                        //FileType = fileExtension,
                        UploadDate = DateTime.Now,
                        ContentType = contentType,
                        Course = course,
                        Module = module,
                        Activity = activity,
                        User = applicationUser
                    };


                    using (var target = new MemoryStream())
                    {
                        file.CopyTo(target);
                        objfiles.Data = target.ToArray();
                    }

                    await unitOfWork.documentRepo.AddDocument(objfiles);
                    await unitOfWork.CompleteAsync();

                    TempData["Success"] = $"{model.Upload.FileName} is successfully uploaded";
                }
            }
        }

        return Redirect(model.Url);


    }



}
