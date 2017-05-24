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
    public class CitiesController : MapUnitsController
    {
        public CitiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        // POST: Cities/Create
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

        protected override async Task<MapUnit> GetUnitByIdAsync(int? id)
        {
            return await _context.Cities.SingleOrDefaultAsync(c => c.ID == id);
        }

        protected override async Task<bool> TryUpdateMapUnitModelAsync(MapUnit model)
        {
            if (!(model is City)) return false;
            return await TryUpdateModelAsync(model as City, "",
                c => c.Name, c => c.X, c => c.Y);
        }
    }
}
