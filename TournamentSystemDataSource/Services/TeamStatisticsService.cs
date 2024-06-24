using Microsoft.EntityFrameworkCore;
using System.Threading;
using TournamentSystemDataSource.Contexts;
using TournamentSystemDataSource.DTO.Statistics;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal sealed class TeamStatisticsService : ITeamStatisticsService
    {
        private readonly GeneralContext _context;
        public TeamStatisticsService(GeneralContext context)
        {
            _context = context;
        }

        public async Task<TeamStatistics> GetTeamStatisticAsync(int teamId, CancellationToken cancellationToken)
        {
            TeamStatistics stats = new TeamStatistics();
            stats.AverageScore = await CalculateAverageTeamScorePerTournamentAsync(teamId, cancellationToken);
            stats.Rating = await CalculateTeamRatingAsync(teamId, cancellationToken);
            stats.MatchesPlayed = await CalculateCountOfMatchesTeamParticipateAsync(teamId, cancellationToken);
            return stats;
        }

        public async Task<double> CalculateAverageTeamScorePerTournamentAsync(int teamId, CancellationToken cancellationToken)
        {
            var matchups = await _context.Matchups.Include(x => x.Entries)
                             .ThenInclude(x => x.TeamCompeting)
                             .AsNoTracking()
                             .ToListAsync(cancellationToken);

            return matchups.SelectMany(m => m.Entries).Where(x => x.TeamCompeting.Id == teamId).Average(g => g.Score);
        }

        public async Task<double> CalculateAverageTeamScorePerTournamentAsync(int tournamentId, int teamId, CancellationToken cancellationToken)
        {
            var matchups = await _context.Matchups.Include(x => x.Entries)
                             .ThenInclude(x => x.TeamCompeting)
                             .AsNoTracking()
                             .Where(m => m.TournamentId == tournamentId)
                             .ToListAsync(cancellationToken);

            return matchups.SelectMany(m => m.Entries).Where(x => x.TeamCompeting.Id == teamId).Average(g => g.Score);
        }

        public async Task<Dictionary<string, double>> CalculateAverageTeamsScoresInTheTournament(int tournamentId, CancellationToken cancellationToken)
        {
            var matchups = await _context.Matchups
                             .Include(x => x.Entries)
                             .ThenInclude(x => x.TeamCompeting)
                             .AsNoTracking()
                             .Where(m => m.TournamentId == tournamentId)
                             .ToListAsync(cancellationToken);

            var result = matchups
                .SelectMany(m => m.Entries)
                .GroupBy(x => x.TeamCompeting.Name)
                .ToDictionary(x => x.Key, x => x.Average(e => e.Score));

            return result;
        }

        public async Task<double> CalculateTeamRatingAsync(int teamId, CancellationToken cancellationToken)
        {
            var matchups = await _context.Matchups
                             .Include(x => x.Entries)
                                .ThenInclude(x => x.TeamCompeting)
                             .Include(x => x.Winner)
                             .AsNoTracking()
                             .ToListAsync(cancellationToken);

            var totalGames = matchups.SelectMany(x => x.Entries).Where(e => e.TeamCompeting.Id == teamId).Count();
            var totalWins = matchups.Where(x => x.Winner.Id == teamId).Count();
            return totalGames > 0 ? (double)totalWins / totalGames : 0;
        }

        public async Task<int> CalculateCountOfMatchesTeamParticipateAsync(int teamId, CancellationToken cancellationToken)
        {
            var matchups = await _context.Matchups
                             .Include(x => x.Entries)
                                .ThenInclude(x => x.TeamCompeting)
                             .AsNoTracking()
                             .ToListAsync(cancellationToken);
            return matchups.SelectMany(x => x.Entries).Where(e => e.TeamCompeting.Id == teamId).Count();
        }
    }
}
