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
        protected override Costs _unitCost
        {
            get
            {
                return Costs.Road;
            }
        }
        public RoadsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        // POST: Roads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Orientation,X,Y")] Road road)
        {
            if (await TryCreateUnit(road))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(road);
            }
        }

        public async Task<JsonResult> CreateFromGraph([Bind("Orientation,X,Y")] Road road)
        {
            return await TryCreateUnitFromGraph(road);
        }

        protected override async Task<MapUnit> GetUnitByIdAsync(int? id)
        {
            return await _context.Roads.SingleOrDefaultAsync(r => r.ID == id);
        }

        protected override async Task<MapUnit> GetUnitByPositionAsync(int x, int y)
        {
            return await _context.Roads.SingleOrDefaultAsync(c => c.X == x && c.Y == y);
        }

        protected override async Task<bool> TryUpdateMapUnitModelAsync(MapUnit model)
        {
            if (!(model is Road)) return false;
            return await TryUpdateModelAsync(model as Road, "",
                c => c.Orientation, c => c.X, c => c.Y);
        }

    }
}
