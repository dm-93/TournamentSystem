using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentSystemDataSource.Contexts;
using TournamentSystemDataSource.DTO.Statistics;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal sealed class TournamentStatisticsService : ITournamentStatisticsService
    {
        private readonly GeneralContext _generalContext;
        private readonly ITeamStatisticsService _teamService;

        public TournamentStatisticsService(GeneralContext generalContext, ITeamStatisticsService service)
        {
            _generalContext = generalContext;
            _teamService = service;
        }

        public async Task<TournamentStatistics> GetTrounamentStatistics(int tournamentId, CancellationToken cancellationToken)
        {
            var statisticsModel = new TournamentStatistics();
            statisticsModel.Start = await GetTournamentStartDateAsync(tournamentId, cancellationToken);
            statisticsModel.End = await GetTournamentEndDateAsync(tournamentId, cancellationToken);
            statisticsModel.Winner = await GetTournamentWinnerAsync(tournamentId, cancellationToken);
            statisticsModel.CountOfMatches = await GetCountOfMatchesAsync(tournamentId, cancellationToken);
            statisticsModel.CountOfRounds = await GetCountOfRoundsInTournament(tournamentId, cancellationToken);
            statisticsModel.TeamAverageScoreStatistic = await _teamService.CalculateAverageTeamsScoresInTheTournament(tournamentId, cancellationToken);
            return statisticsModel;
        }

        private async Task<int> GetCountOfRoundsInTournament(int tournamentId, CancellationToken cancellationToken)
        {
            return await _generalContext.Matchups.AsNoTracking()
                                                 .Where(m => m.TournamentId == tournamentId)
                                                 .Select(m => m.MatchupRound)
                                                 .Distinct()
                                                 .CountAsync(cancellationToken);
        }

        private async Task<int> GetCountOfMatchesAsync(int tournamentId, CancellationToken cancellationToken)
        {
            return await _generalContext.Matchups.AsNoTracking()
                                                .Where(m => m.TournamentId == tournamentId)
                                                .CountAsync(cancellationToken);
        }

        private async Task<Team> GetTournamentWinnerAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var final = await _generalContext.Matchups
                                             .Include(t => t.Winner)
                                             .AsNoTracking()
                                             .Where(t => t.TournamentId == tournamentId)
                                             .OrderByDescending(x => x.MatchupRound)
                                             .FirstOrDefaultAsync(cancellationToken);
            return final.Winner;
        }

        private async Task<DateTime> GetTournamentEndDateAsync(int tournamentId, CancellationToken cancellationToken)
        {
            return await _generalContext.Tournaments.AsNoTracking()
                                                    .Where(x => x.Id == tournamentId)
                                                    .Select(t => t.EndDate)
                                                    .FirstOrDefaultAsync(cancellationToken);
        }

        private async Task<DateTime> GetTournamentStartDateAsync(int tournamentId, CancellationToken cancellationToken)
        {
            return await _generalContext.Tournaments.AsNoTracking()
                                                    .Where(x => x.Id == tournamentId)
                                                    .Select(t => t.StartDate)
                                                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
