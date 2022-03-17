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
using Lms.Core.Interfaces;
using AutoMapper;
using Lms.Core.Models;

namespace Lms.Web.Controllers
{
    public class ActivityTypesController : Controller
    {
        private readonly IMapper mapper;

        //private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork uow;
        public ActivityTypesController(IMapper mapper, IUnitOfWork unitofwork )
        {
            this.mapper = mapper;
            this.uow = unitofwork;
        }
        public async Task<IActionResult> CourseOverView()
        {
            var modules = await uow.moduleRepo.GetAllModulesBy_courseId(1);
            return View(mapper.Map<CourseOverViewModel>(modules));
        }

//        public ActivityTypesController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: ActivityTypes
//        public async Task<IActionResult> Index()
//        {
//            return View(await _context.ActivityTypes.ToListAsync());
//        }

//        // GET: ActivityTypes/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var activityType = await _context.ActivityTypes
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (activityType == null)
//            {
//                return NotFound();
//            }

//            return View(activityType);
//        }

//        // GET: ActivityTypes/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: ActivityTypes/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,Name")] ActivityType activityType)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(activityType);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(activityType);
//        }

//        // GET: ActivityTypes/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var activityType = await _context.ActivityTypes.FindAsync(id);
//            if (activityType == null)
//            {
//                return NotFound();
//            }
//            return View(activityType);
//        }

//        // POST: ActivityTypes/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ActivityType activityType)
//        {
//            if (id != activityType.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(activityType);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ActivityTypeExists(activityType.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(activityType);
//        }

//        // GET: ActivityTypes/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var activityType = await _context.ActivityTypes
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (activityType == null)
//            {
//                return NotFound();
//            }

//            return View(activityType);
//        }

//        // POST: ActivityTypes/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var activityType = await _context.ActivityTypes.FindAsync(id);
//            _context.ActivityTypes.Remove(activityType);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ActivityTypeExists(int id)
//        {
//            return _context.ActivityTypes.Any(e => e.Id == id);
//        }
    }
}
