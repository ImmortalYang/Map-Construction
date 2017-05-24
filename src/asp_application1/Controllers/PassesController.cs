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
    public class PassesController : MapUnitsController
    {
        public PassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        // POST: Passes/Create
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

        protected override async Task<MapUnit> GetUnitByIdAsync(int? id)
        {
            return await _context.Passes.SingleOrDefaultAsync(p => p.ID == id);
        }

        protected override async Task<bool> TryUpdateMapUnitModelAsync(MapUnit model)
        {
            if (!(model is Pass)) return false;
            return await TryUpdateModelAsync(model as Pass, "",
                c => c.Duration, c => c.X, c => c.Y);
        }
    }
}
