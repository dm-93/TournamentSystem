using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TournamentSystemDataSource.Contexts;
using TournamentSystemDataSource.Repositories.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Repositories
{
    internal sealed class RoundsRepository : IRoundsRepository
    {
        private readonly GeneralContext _context;
        public RoundsRepository(GeneralContext context)
        {
            _context = context;
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
