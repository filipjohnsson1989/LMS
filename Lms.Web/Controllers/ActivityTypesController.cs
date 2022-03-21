#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lms.Core.Entities;
using Lms.Data.Data;
using Lms.Web.Models;

namespace Lms.Web.Controllers
{
    public class ActivityTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivityTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActivityTypes
        public async Task<IActionResult> Index()
        {
            var activityTypes = await _context.ActivityTypes.ToListAsync();
            var docs = await _context.Documents.ToListAsync();
            UpDownLoadCombinedTestViewModel testModel = new UpDownLoadCombinedTestViewModel();
            testModel.Documents = docs;
            testModel.activityTypes = activityTypes;
            return View(testModel);
            
        }

        // GET: ActivityTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityTypes
                .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Add(activityType);
                await _context.SaveChangesAsync();
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

            var activityType = await _context.ActivityTypes.FindAsync(id);
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
                    _context.Update(activityType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityTypeExists(activityType.Id))
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

            var activityType = await _context.ActivityTypes
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var activityType = await _context.ActivityTypes.FindAsync(id);
            _context.ActivityTypes.Remove(activityType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityTypeExists(int id)
        {
            return _context.ActivityTypes.Any(e => e.Id == id);
        }

        public async Task<FileResult> DownloadAsync(int id)
        {
            var fileToRetrieve = await _context.Documents.FirstAsync(d => d.Id == id);
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

                        var objfiles = new Document()
                        {
                            Name = newFileName,
                            //FileType = fileExtension,
                            UploadDate = DateTime.Now,
                            ContentType = contentType
                        };


                        using (var target = new MemoryStream())
                        {
                            file.CopyTo(target);
                            objfiles.Data = target.ToArray();
                        }

                        _context.Documents.Add(objfiles);
                        await _context.SaveChangesAsync();

                    }
                }
            }
            return RedirectToAction("Index");
        }



    }
}
