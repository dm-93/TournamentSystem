using TournamentSystemDataSource.DTO.Pagination;
using TournamentSystemDataSource.DTO.Rounds;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface IRoundsService
    {
        Task CreateRoundsAsync(Tournament tournament, CancellationToken cancellationToken);
        Task GenerateRoundAsync(int tournamentId, int prevRoundId, CancellationToken cancellationToken);
        Task<PaginationResponse<IEnumerable<Matchup>>> GetMatchupsAsync(Pagination<GetNextRoundDto> pagination, CancellationToken cancellationToken);
        Task<IEnumerable<Matchup>> GetRoundAsync(int tournamentId, int nextRoundId, CancellationToken cancellationToken);
        Task UpdateMatchAsync(UpdateRequest matchup, CancellationToken cancellationToken);
    }
}