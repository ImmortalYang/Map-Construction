using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using asp_application1.Data;
using asp_application1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace asp_application1.Controllers
{
    [Authorize(Roles = ("Admin,Member"))]
    public class PassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public PassesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Passes
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var passes = await _context.Passes
                .Where(c => c.ApplicationUserId == user.Id)
                .AsNoTracking()
                .ToListAsync();
            return View(passes);
        }

        // GET: Passes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pass = await _context.Passes
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (pass == null)
            {
                return NotFound();
            }

            return View(pass);
        }

        // GET: Passes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Passes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Duration,X,Y")] Pass pass)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                pass.Cost = Costs.Pass;
                pass.ApplicationUserId = user.Id;
                if (await pass.UnitLocationOccupied(_context, user))
                {
                    ModelState.AddModelError("", "The location is occupied by another unit.");
                    return View(pass);
                }
                if (user.Balance < (int)pass.Cost)
                {
                    ModelState.AddModelError("", "You have insufficient balance to build this unit.");
                    return View(pass);
                }
                user.Balance -= (int)pass.Cost;
                _context.Add(pass);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(pass);
        }

        // GET: Passes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pass = await _context.Passes
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (pass == null)
            {
                return NotFound();
            }
            return View(pass);
        }

        // POST: Passes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var passToUpdate = await _context.Passes
                .SingleOrDefaultAsync(c => c.ID == id);
            if (await TryUpdateModelAsync(passToUpdate,
                "",
                c => c.Duration, c => c.X, c => c.Y))
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (await passToUpdate.UnitLocationOccupied(_context, user))
                    {
                        ModelState.AddModelError("", "The location is occupied by another unit.");
                        return View(passToUpdate);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
                }
            }

            return View(passToUpdate);
        }

        // GET: Passes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pass = await _context.Passes
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (pass == null)
            {
                return NotFound();
            }

            return View(pass);
        }

        // POST: Passes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passToDelete = new Pass() { ID = id };
            _context.Entry(passToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PassExists(int id)
        {
            return _context.Passes.Any(e => e.ID == id);
        }
    }
}
