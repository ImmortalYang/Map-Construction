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
        protected override Costs _unitCost
        {
            get
            {
                return Costs.Pass;
            }
        }

        public PassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        // POST: Passes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Duration,X,Y")] Pass pass)
        {
            if (await TryCreateUnit(pass))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(pass);
            }
        }

        public async Task<JsonResult> CreateFromGraph([Bind("Duration,X,Y")] Pass pass)
        {
            return await TryCreateUnitFromGraph(pass);
        }

        protected override async Task<MapUnit> GetUnitByIdAsync(int? id)
        {
            return await _context.Passes.SingleOrDefaultAsync(p => p.ID == id);
        }

        protected override async Task<MapUnit> GetUnitByPositionAsync(int x, int y)
        {
            return await _context.Passes.SingleOrDefaultAsync(c => c.X == x && c.Y == y);
        }

        protected override async Task<bool> TryUpdateMapUnitModelAsync(MapUnit model)
        {
            if (!(model is Pass)) return false;
            return await TryUpdateModelAsync(model as Pass, "",
                c => c.Duration, c => c.X, c => c.Y);
        }
    }
}
