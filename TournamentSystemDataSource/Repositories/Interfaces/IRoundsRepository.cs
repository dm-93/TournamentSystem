using TournamentSystemDataSource.DTO.Pagination;
using TournamentSystemDataSource.DTO.Rounds;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Repositories.Interfaces
{
    public interface IRoundsRepository
    {
        Task<Matchup?> GetById(int matchupId, CancellationToken cancellationToken);
        Task<PaginationResponse<IEnumerable<Matchup>>> GetTournamentRoundsAsync(Pagination<GetNextRoundDto> pagination, CancellationToken cancellationToken);
        Task<List<Matchup>> GetTournamentRoundsAsync(int tournamentId, CancellationToken cancellationToken);
        Task SaveRoundsAsync(Tournament tournament);
        Task UpdateMatchupAsync(Matchup matchup, CancellationToken cancellationToken);
    }
}