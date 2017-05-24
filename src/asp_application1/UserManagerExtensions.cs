using asp_application1.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace asp_application1
{
    public static class UserManagerExtensions
    {
        /// <summary>
        /// Extension method: get the balance of a user from claims principal
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static async Task<double> GetUserBalanceAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            return user.Balance;
        }
    }
}
