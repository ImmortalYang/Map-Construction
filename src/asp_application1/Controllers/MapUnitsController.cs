using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using asp_application1.Models.MapUnitViewModels;
using asp_application1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using asp_application1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace asp_application1.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class MapUnitsController : Controller
    {
        protected readonly ApplicationDbContext _context;
        protected UserManager<ApplicationUser> _userManager;
        protected virtual Costs _unitCost { get; }

        public MapUnitsController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View("~/Views/MapUnits/ListIndex.cshtml", 
                await GetIndexViewModelAsync());
        }

        public async Task<IActionResult> GraphicIndex()
        {
            return View("~/Views/MapUnits/GraphicIndex.cshtml",
                await GetIndexViewModelAsync());
        }

        public async Task<IActionResult> FilterIndex(MapUnitIndexViewModel model)
        {
            return View("~/Views/MapUnits/FilterIndex.cshtml",
                await GetIndexViewModelAsync(model.ShowCity, model.ShowRoad, model.ShowPass));
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<bool> TryCreateUnit(MapUnit unit)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                unit.Cost = _unitCost;
                unit.ApplicationUserId = user.Id;
                if (await unit.UnitLocationOccupied(_context, user))
                {
                    ModelState.AddModelError("", "The location is occupied by another unit.");
                    return false;
                }
                if (user.Balance < (int)unit.Cost)
                {
                    ModelState.AddModelError("", "You have insufficient balance to build this unit.");
                    return false;
                }
                user.Balance -= (int)unit.Cost;
                try
                {
                    _context.Add(unit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes, please try again.");
                    return false;
                }

                return true;
            }
            return false;
        }

        public async Task<JsonResult> TryCreateUnitFromGraph(MapUnit unit)
        {
            if (await TryCreateUnit(unit))
            {
                return Json("success");
            }
            else
            {
                return Json(ModelState.Values
                            .Where(entry => entry.Errors.Count > 0)
                            .SelectMany(entry => entry.Errors)
                            .Select(error => error.ErrorMessage));
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var unit = await GetUnitByIdAsync(id);
            if (unit == null)
            {
                return NotFound();
            }
            return View(unit);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await GetUnitByIdAsync(id);
            if (unit == null)
            {
                return NotFound();
            }
            return View(unit);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var unitToUpdate = await GetUnitByIdAsync(id);
            if (await TryUpdateMapUnitModelAsync(unitToUpdate))
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (await unitToUpdate.UnitLocationOccupied(_context, user))
                    {
                        ModelState.AddModelError("", "The location is occupied by another unit.");
                        return View(unitToUpdate);
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

            return View(unitToUpdate);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await GetUnitByIdAsync(id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unitToDelete = await GetUnitByIdAsync(id);
            _context.Remove(unitToDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> DeleteFromGraph(int x, int y)
        {
            var unitToDelete = await GetUnitByPositionAsync(x, y);
            if (unitToDelete == null)
            {
                return Json("unit not found.");
            }
            try
            {
                _context.Remove(unitToDelete);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Json("Data update failed. Please try again.");
            }
            return Json("success");
        }

        protected virtual Task<MapUnit> GetUnitByPositionAsync(int x, int y)
        {
            throw new NotImplementedException();
        }

        protected virtual Task<MapUnit> GetUnitByIdAsync(int? id)
        {
            throw new NotImplementedException();
        }

        protected virtual Task<bool> TryUpdateMapUnitModelAsync(MapUnit model)
        {
            throw new NotImplementedException();
        }

        private async Task<MapUnitIndexViewModel> GetIndexViewModelAsync(
                                            bool showCity = true, 
                                            bool showRoad = true, 
                                            bool showPass = true)
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new MapUnitIndexViewModel()
            {
                ShowCity = showCity, 
                ShowRoad = showRoad, 
                ShowPass = showPass, 
                Cities = showCity ? await _context.Cities
                    .Where(c => c.ApplicationUserId == user.Id)
                    .AsNoTracking().ToListAsync() : new List<City>(),
                Roads = showRoad ? await _context.Roads
                    .Where(r => r.ApplicationUserId == user.Id)
                    .AsNoTracking().ToListAsync() : new List<Road>(),
                Passes = showPass ? await _context.Passes
                    .Where(p => p.ApplicationUserId == user.Id)
                    .AsNoTracking().ToListAsync() : new List<Pass>()
            };
            return model;
        }

        public async Task<JsonResult> GetIndexViewModelJsonAsync()
        {
            return Json(await GetIndexViewModelAsync());
        }
    }
}