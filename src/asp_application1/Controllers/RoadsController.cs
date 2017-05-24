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
    public class RoadsController : MapUnitsController
    {
        public RoadsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        // POST: Roads/Create
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

        protected override async Task<MapUnit> GetUnitByIdAsync(int? id)
        {
            return await _context.Roads.SingleOrDefaultAsync(r => r.ID == id);
        }

        protected override async Task<bool> TryUpdateMapUnitModelAsync(MapUnit model)
        {
            if (!(model is Road)) return false;
            return await TryUpdateModelAsync(model as Road, "",
                c => c.Orientation, c => c.X, c => c.Y);
        }

    }
}
