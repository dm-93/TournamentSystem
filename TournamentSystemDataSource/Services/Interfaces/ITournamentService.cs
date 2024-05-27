using TournamentSystemDataSource.DTO;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface ITournamentService
    {
        Task<Tournament> CreateTournamentAsync(Tournament tournament, CancellationToken cancellationToken);
        Task DeleteTournamentAsync(int tournamentId, CancellationToken cancellationToken);
        Task<IEnumerable<TournamentDto>> GetTournamentByConditionAsync(GetByConditionRequest request, CancellationToken cancellationToken);
        Task<IEnumerable<TournamentDto>> GetTournamentsAsync(CancellationToken cancellationToken);
        Task<Tournament> UpdateTournamentAsync(TournamentDto updatedTournament, CancellationToken cancellationToken);
    }
}