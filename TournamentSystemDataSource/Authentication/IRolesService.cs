using Microsoft.AspNetCore.Identity;

namespace TournamentSystemDataSource.Authentication
{
    public interface IRolesService
    {
        Task<IdentityResult> AddRoleAsync(string roleName);
    }
}