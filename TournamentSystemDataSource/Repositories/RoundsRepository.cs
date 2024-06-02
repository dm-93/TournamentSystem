using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TournamentSystemDataSource.Contexts;
using TournamentSystemDataSource.DTO.Pagination;
using TournamentSystemDataSource.DTO.Rounds;
using TournamentSystemDataSource.Repositories.Interfaces;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TournamentSystemDataSource.Repositories
{
    internal sealed class RoundsRepository : IRoundsRepository
    {
        private readonly GeneralContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public RoundsRepository(GeneralContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Matchup?> GetById(int matchupId, CancellationToken cancellationToken)
        {
            return await _context.Matchups
                .AsNoTracking()
                .Include(x => x.Entries)
                    .ThenInclude(e => e.TeamCompeting)
                .Include(m => m.Winner)
                .FirstOrDefaultAsync(m => m.Id == matchupId, cancellationToken);
        }

        private async Task UpdateMatchAsync(SqlConnection connection,
                                            Matchup matchup,
                                            SqlTransaction transaction)
        {
            var query = @"UPDATE Matchups
                        SET WinnerId = @WinnerId, UpdatedOn = @UpdatedOn
                        WHERE Id = @Id;"
            ;

            using (var command = new SqlCommand(query, connection, transaction))
            {
               
                command.Parameters.AddWithValue("@WinnerId", (object)matchup.Winner?.Id ?? DBNull.Value);
                command.Parameters.AddWithValue("@UpdatedOn", ValidateDateTime(matchup.UpdatedOn));
                command.Parameters.AddWithValue("@Id", matchup.Id);
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task UpdateMatchEntryAsync(SqlConnection connection,
                                            MatchupEntry entry,
                                            SqlTransaction transaction)
        {
            var query = @"UPDATE MatchupEntries
                        SET Score = @Score, TeamCompetingId = @TeamCompetingId
                        WHERE Id = @Id;"
            ;

            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@Score", entry.Score);
                command.Parameters.AddWithValue("@TeamCompetingId", (object)entry.TeamCompeting?.Id ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", entry.Id);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateMatchupAsync(Matchup matchup, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                await UpdateMatchAsync(connection, matchup, transaction);

                foreach (var entry in matchup.Entries)
                {
                    entry.ParentMatchup = matchup;
                    await UpdateMatchEntryAsync(connection, entry, transaction);
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task SaveRoundsAsync(Tournament tournament)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                foreach (var round in tournament.RoundsNm)
                {
                    foreach (var matchup in round)
                    {
                        var matchupId = await InsertMatchupAsync(connection, matchup, tournament.Id, transaction);

                        foreach (var entry in matchup.Entries)
                        {
                            entry.ParentMatchup = matchup;
                            await InsertMatchupEntryAsync(connection, entry, matchupId, transaction);
                        }
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Matchup>> GetTournamentRoundsAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var data = await _context.Matchups
                .Include(x => x.Entries)
                    .ThenInclude(e => e.TeamCompeting)
                .Include(m => m.Winner)
                .Where(m => m.TournamentId == tournamentId)
                .ToListAsync(cancellationToken);
            return data ?? new List<Matchup>();
        }

        public async Task<PaginationResponse<IEnumerable<Matchup>>> GetTournamentRoundsAsync(Pagination<GetNextRoundDto> pagination, CancellationToken cancellationToken)
        {
            var totalCount = await _context.Matchups.CountAsync(cancellationToken);
            var data = await _context.Matchups
                .Include(x => x.Entries)
                    .ThenInclude(e => e.TeamCompeting)
                .Include(m => m.Winner)
                .Where(m => m.TournamentId == pagination.Parameter.tournamentId && m.MatchupRound == pagination.Parameter.roundId)
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .ToListAsync(cancellationToken);
            return new PaginationResponse<IEnumerable<Matchup>> { Data = data, TotalCount = totalCount };
        }

        private async Task<int> InsertMatchupAsync(SqlConnection connection, Matchup matchup, int tournamentId, SqlTransaction transaction)
        {
            var query = @"INSERT INTO Matchups (CreatedOn, UpdatedOn, Deleted, MatchupRound, WinnerId, TournamentId)
                  OUTPUT INSERTED.Id
                  VALUES (@CreatedOn, @UpdatedOn, @Deleted, @MatchupRound, @WinnerId, @TournamentId)";

            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@CreatedOn", ValidateDateTime(matchup.CreatedOn));
                command.Parameters.AddWithValue("@UpdatedOn", ValidateDateTime(matchup.UpdatedOn));
                command.Parameters.AddWithValue("@Deleted", matchup.Deleted);
                command.Parameters.AddWithValue("@MatchupRound", matchup.MatchupRound);
                command.Parameters.AddWithValue("@WinnerId", (object)matchup.Winner?.Id ?? DBNull.Value);
                command.Parameters.AddWithValue("@TournamentId", tournamentId);

                return (int)await command.ExecuteScalarAsync();
            }
        }

        private async Task InsertMatchupEntryAsync(SqlConnection connection, MatchupEntry entry, int parentMatchupId, SqlTransaction transaction)
        {
            var query = @"INSERT INTO MatchupEntries (TeamCompetingId, Score, ParentMatchupId)
                  VALUES (@TeamCompetingId, @Score, @ParentMatchupId)";

            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TeamCompetingId", (object)entry.TeamCompeting?.Id ?? DBNull.Value);
                command.Parameters.AddWithValue("@Score", entry.Score);
                command.Parameters.AddWithValue("@ParentMatchupId", parentMatchupId);

                await command.ExecuteNonQueryAsync();
            }
        }

        private DateTime ValidateDateTime(DateTime dateTime)
        {
            DateTime defaultValue = DateTime.UtcNow;

            if (dateTime == default(DateTime))
            {
                return defaultValue;
            }

            return dateTime;
        }
    }
}
