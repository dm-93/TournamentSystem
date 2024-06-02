using TournamentSystemDataSource.DTO.Pagination;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface IRoundsService
    {
        Task CreateRoundsAsync(Tournament tournament, CancellationToken cancellationToken);
        Task<PaginationResponse<IEnumerable<Matchup>>> GetMatchupsAsync(Pagination<int> pagination, CancellationToken cancellationToken);
    }
}