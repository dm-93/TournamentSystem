using TournamentSystemDataSource.DTO.Pagination;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Repositories.Interfaces
{
    public interface IRoundsRepository
    {
        Task<PaginationResponse<IEnumerable<Matchup>>> GetTournamentRoundsAsync(Pagination<int> pagination, CancellationToken cancellationToken);
        Task SaveRoundsAsync(Tournament tournament);
    }
}