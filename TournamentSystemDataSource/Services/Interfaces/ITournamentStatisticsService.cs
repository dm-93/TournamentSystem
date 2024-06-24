using TournamentSystemDataSource.DTO.Statistics;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface ITournamentStatisticsService
    {
        Task<TournamentStatistics> GetTrounamentStatistics(int tournamentId, CancellationToken cancellationToken);
    }
}