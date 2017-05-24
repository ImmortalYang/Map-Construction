using asp_application1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace asp_application1.Models
{
    public class MapUnit
    {
        public int ID { get; set; }
        public string ApplicationUserId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Please enter a number between 0 and 100.")]
        public int X { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Please enter a number between 0 and 100.")]
        public int Y { get; set; }

        public Costs Cost { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// Check asynchronously if the location(X, Y) of the unit is already occupied by another unit
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="user">Current user</param>
        /// <returns></returns>
        public async Task<bool> UnitLocationOccupied(ApplicationDbContext context, ApplicationUser user)
        {
            var locationOccupiedByCity = await context.Cities
                .Where(c => c.ApplicationUserId == user.Id)
                .Where(c => c.X == this.X && c.Y == this.Y)
                .Where(c => c.ID != this.ID)
                .AsNoTracking().AnyAsync();
            var locationOccupiedByRoad = await context.Roads
                .Where(c => c.ApplicationUserId == user.Id)
                .Where(c => c.X == this.X && c.Y == this.Y)
                .Where(c => c.ID != this.ID)
                .AsNoTracking().AnyAsync();
            var locationOccupiedByPass = await context.Passes
                .Where(c => c.ApplicationUserId == user.Id)
                .Where(c => c.X == this.X && c.Y == this.Y)
                .Where(c => c.ID != this.ID)
                .AsNoTracking().AnyAsync();
            if(locationOccupiedByCity || locationOccupiedByRoad || locationOccupiedByPass)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public enum Costs
    {
        City = 20, 
        Road = 5, 
        Pass = 10
    }
}
