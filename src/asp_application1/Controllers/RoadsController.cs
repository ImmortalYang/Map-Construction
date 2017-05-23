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

namespace asp_application1.Controllers
{
    [Authorize(Roles = ("Admin,Member"))]
    public class RoadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoadsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Roads
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Roads.Include(r => r.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Roads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var road = await _context.Roads.SingleOrDefaultAsync(m => m.ID == id);
            if (road == null)
            {
                return NotFound();
            }

            return View(road);
        }

        // GET: Roads/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Roads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ApplicationUserId,Cost,Orientation,X,Y")] Road road)
        {
            if (ModelState.IsValid)
            {
                _context.Add(road);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", road.ApplicationUserId);
            return View(road);
        }

        // GET: Roads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var road = await _context.Roads.SingleOrDefaultAsync(m => m.ID == id);
            if (road == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", road.ApplicationUserId);
            return View(road);
        }

        // POST: Roads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ApplicationUserId,Cost,Orientation,X,Y")] Road road)
        {
            if (id != road.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(road);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoadExists(road.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", road.ApplicationUserId);
            return View(road);
        }

        // GET: Roads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var road = await _context.Roads.SingleOrDefaultAsync(m => m.ID == id);
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
            var road = await _context.Roads.SingleOrDefaultAsync(m => m.ID == id);
            _context.Roads.Remove(road);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RoadExists(int id)
        {
            return _context.Roads.Any(e => e.ID == id);
        }
    }
}
