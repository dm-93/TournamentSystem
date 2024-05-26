using TournamentSystemDataSource.DTO;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface ITeamService
    {
        Task<Team> CreateTeamAsync(Team team, CancellationToken cancellationToken);
        Task DeleteTeamAsync(int teamId, CancellationToken cancellationToken);
        Task<Team> GetTeamByIdAsync(int teamId, CancellationToken cancellationToken);
        Task<IEnumerable<Team>> GetTeamsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Team>> GetTeamsByConditionAsync(GetByConditionRequest request, CancellationToken cancellationToken);
        Task<Team> UpdateTeamAsync(Team updatedTeam, CancellationToken cancellationToken);
    }
}