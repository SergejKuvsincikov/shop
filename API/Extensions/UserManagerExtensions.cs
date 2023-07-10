using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByEmailWithAddressAsync(this UserManager<AppUser> input,
            ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email).Value;
            var userWithAddress = await input.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
            return userWithAddress;
        }

        public static async Task<AppUser> FindByEmailFromClaimsPrincipalAsync(this UserManager<AppUser> input,
            ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email).Value;
            var userWithoutAddress = await input.Users.SingleOrDefaultAsync(x => x.Email == email);
            return userWithoutAddress;
        }
    }
}