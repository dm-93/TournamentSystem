using TournamentSystemDataSource.DTO;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface ITournamentService
    {
        Task CompleteTournamentAsync(int tournamentId, CancellationToken cancellationToken);
        Task<TournamentDto> CreateTournamentAsync(TournamentDto dto, CancellationToken cancellationToken);
        Task DeleteTournamentAsync(int tournamentId, CancellationToken cancellationToken);
        Task<Tournament?> GetByIdAsync(int tournamentId, CancellationToken cancellationToken);
        Task<IEnumerable<TournamentDto>> GetTournamentByConditionAsync(GetByConditionRequest request, CancellationToken cancellationToken);
        Task<IEnumerable<TournamentDto>> GetTournamentsAsync(CancellationToken cancellationToken);
        Task GetTournamentsByUserEmailAsync(string email, CancellationToken cancellationToken);
        Task<Tournament> UpdateTournamentAsync(TournamentDto updatedTournament, CancellationToken cancellationToken);
    }
}