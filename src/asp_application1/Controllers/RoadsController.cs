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
    public class RoadsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public RoadsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;  
        }

        // GET: Roads
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var roads = await _context.Roads
                .Where(r => r.ApplicationUserId == user.Id)
                .AsNoTracking()
                .ToListAsync();
            return View(roads);
        }

        // GET: Roads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var road = await _context.Roads
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (road == null)
            {
                return NotFound();
            }

            return View(road);
        }

        // GET: Roads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Orientation,X,Y")] Road road)
        {  
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                road.Cost = Costs.Road;
                road.ApplicationUserId = user.Id;
                if (await road.UnitLocationOccupied(_context, user))
                {
                    ModelState.AddModelError("", "The location is occupied by another unit.");
                    return View(road);
                }
                if (user.Balance < (int)road.Cost)
                {
                    ModelState.AddModelError("", "You have insufficient balance to build this unit.");
                    return View(road);
                }
                user.Balance -= (int)road.Cost;
                _context.Add(road);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(road);
        }

        // GET: Roads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var road = await _context.Roads
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (road == null)
            {
                return NotFound();
            }
            return View(road);
        }

        // POST: Roads/Edit/5
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

            var roadToUpdate = await _context.Roads
                .SingleOrDefaultAsync(c => c.ID == id);
            if (await TryUpdateModelAsync(roadToUpdate,
                "",
                c => c.Orientation, c => c.X, c => c.Y))
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (await roadToUpdate.UnitLocationOccupied(_context, user))
                    {
                        ModelState.AddModelError("", "The location is occupied by another unit.");
                        return View(roadToUpdate);
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
            return View(roadToUpdate);
        }

        // GET: Roads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var road = await _context.Roads
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (road == null)
            {
                return NotFound();
            }

            return View(road);
        }

        // POST: Roads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roadToDelete = new Road() { ID = id };
            _context.Entry(roadToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RoadExists(int id)
        {
            return _context.Roads.Any(e => e.ID == id);
        }
    }
}
