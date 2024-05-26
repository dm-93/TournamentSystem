using TournamentSystemDataSource.DTO;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface ITournamentService
    {
        Task<Tournament> CreateTournamentAsync(Tournament tournament, CancellationToken cancellationToken);
        Task DeleteTournamentAsync(int tournamentId, CancellationToken cancellationToken);
        Task<IEnumerable<Tournament>> GetTournamentByConditionAsync(GetByConditionRequest request, CancellationToken cancellationToken);
        Task<IEnumerable<Tournament>> GetTournamentsAsync(CancellationToken cancellationToken);
        Task<Tournament> UpdateTournamentAsync(TournamentDto updatedTournament, CancellationToken cancellationToken);
    }
}