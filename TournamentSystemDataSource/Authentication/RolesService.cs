using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentSystemDataSource.Authentication
{
    internal sealed class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddRoleAsync(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            var role = new IdentityRole { Name = roleName };
            return await _roleManager.CreateAsync(role);
        }

    }
}
