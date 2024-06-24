using TournamentSystemDataSource.DTO.Statistics;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface ITeamStatisticsService
    {
        Task<double> CalculateAverageTeamScorePerTournamentAsync(int teamId, CancellationToken cancellationToken);
        Task<double> CalculateAverageTeamScorePerTournamentAsync(int tournamentId, int teamId, CancellationToken cancellationToken);
        Task<Dictionary<string, double>> CalculateAverageTeamsScoresInTheTournament(int tournamentId, CancellationToken cancellationToken);
        Task<int> CalculateCountOfMatchesTeamParticipateAsync(int teamId, CancellationToken cancellationToken);
        Task<double> CalculateTeamRatingAsync(int teamId, CancellationToken cancellationToken);
        Task<TeamStatistics> GetTeamStatisticAsync(int teamId, CancellationToken cancellationToken);
    }
}