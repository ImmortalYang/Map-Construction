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
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace asp_application1.Controllers
{
    [Authorize(Roles = ("Admin,Member"))]
    public class CitiesController : MapUnitsController
    {
        protected override Costs _unitCost
        {
            get
            {
                return Costs.City;
            }
        }

        public CitiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        // POST: Cities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,X,Y")] City city)
        {
            if (await TryCreateUnit(city))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(city);
            }
        }

        public async Task<JsonResult> CreateFromGraph([Bind("Name,X,Y")] City city)
        {
            return await TryCreateUnitFromGraph(city);
        }

        protected override async Task<MapUnit> GetUnitByIdAsync(int? id)
        {
            return await _context.Cities.SingleOrDefaultAsync(c => c.ID == id);
        }

        protected override async Task<MapUnit> GetUnitByPositionAsync(int x, int y)
        {
            return await _context.Cities.SingleOrDefaultAsync(c => c.X == x && c.Y == y);
        }

        protected override async Task<bool> TryUpdateMapUnitModelAsync(MapUnit model)
        {
            if (!(model is City)) return false;
            return await TryUpdateModelAsync(model as City, "",
                c => c.Name, c => c.X, c => c.Y);
        }
    }
}
