using TournamentSystemModels;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface ITeamDescriptionService
    {
        Task<TeamDescription> CreateTeamDescriptionAsync(TeamDescription teamDescription, CancellationToken cancellationToken);
        Task DeleteTeamDescriptionAsync(int teamDescriptionId, CancellationToken cancellationToken);
        Task<TeamDescription> GetTeamDescriptionByIdAsync(int teamDescriptionId, CancellationToken cancellationToken);
        Task<IEnumerable<TeamDescription>> GetTeamDescriptionsAsync(CancellationToken cancellationToken);
        Task<TeamDescription> UpdateTeamDescriptionAsync(TeamDescription updatedTeamDescription, CancellationToken cancellationToken);
    }
}