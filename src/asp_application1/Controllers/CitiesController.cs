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
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public CitiesController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;    
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cities = await _context.Cities
                .Where(c => c.ApplicationUserId == user.Id)
                .AsNoTracking()
                .ToListAsync();  
            return View(cities);
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,X,Y")] City city)
        { 
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                city.Cost = Costs.City;
                city.ApplicationUserId = user.Id;
                if(await city.UnitLocationOccupied(_context, user))
                {
                    ModelState.AddModelError("", "The location is occupied by another unit.");
                    return View(city);
                }
                if(user.Balance < (int)city.Cost)
                {
                    ModelState.AddModelError("", "You have insufficient balance to build this unit.");
                    return View(city);
                }
                user.Balance -= (int)city.Cost;
                _context.Add(city);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        // POST: Cities/Edit/5
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
            var cityToUpdate = await _context.Cities
                .SingleOrDefaultAsync(c => c.ID == id);
            if(await TryUpdateModelAsync(cityToUpdate, 
                "", 
                c => c.Name, c => c.X, c => c.Y))
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (await cityToUpdate.UnitLocationOccupied(_context, user))
                    {
                        ModelState.AddModelError("", "The location is occupied by another unit.");
                        return View(cityToUpdate);
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
            
            return View(cityToUpdate);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cityToDelete = new City() { ID = id };
            _context.Entry(cityToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.ID == id);
        }
    }
}
