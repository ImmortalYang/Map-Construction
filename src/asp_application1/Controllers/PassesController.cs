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
    public class PassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PassesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Passes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Passes.Include(p => p.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Passes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pass = await _context.Passes.SingleOrDefaultAsync(m => m.ID == id);
            if (pass == null)
            {
                return NotFound();
            }

            return View(pass);
        }

        // GET: Passes/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Passes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ApplicationUserId,Cost,Duration,X,Y")] Pass pass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pass);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", pass.ApplicationUserId);
            return View(pass);
        }

        // GET: Passes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pass = await _context.Passes.SingleOrDefaultAsync(m => m.ID == id);
            if (pass == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", pass.ApplicationUserId);
            return View(pass);
        }

        // POST: Passes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ApplicationUserId,Cost,Duration,X,Y")] Pass pass)
        {
            if (id != pass.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassExists(pass.ID))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", pass.ApplicationUserId);
            return View(pass);
        }

        // GET: Passes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pass = await _context.Passes.SingleOrDefaultAsync(m => m.ID == id);
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
            var pass = await _context.Passes.SingleOrDefaultAsync(m => m.ID == id);
            _context.Passes.Remove(pass);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PassExists(int id)
        {
            return _context.Passes.Any(e => e.ID == id);
        }
    }
}
