using Microsoft.AspNetCore.Identity;
using TournamentSystemModels.Identity;

namespace TournamentSystemDataSource.DTO.Admin
{
    public sealed class GetUsersResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
