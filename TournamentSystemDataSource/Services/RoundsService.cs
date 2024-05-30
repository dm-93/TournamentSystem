using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TournamentSystemDataSource.Contexts;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal sealed class RoundsService : IRoundsService
    {
        private readonly GeneralContext _context;

        public RoundsService(GeneralContext context)
        {
            _context = context;
        }

        public async Task CreateRoundsAsync(Tournament tournament, CancellationToken cancellationToken)
        {
            var randomizedTeams = RandomizeTeamOrder(tournament.EnteredTeams);
            var rounds = FindNumberOfRounds(randomizedTeams);
            var byes = NumberOfByes(rounds, randomizedTeams.Count);
            tournament.RoundsNm.Add(CreateFirstRound(byes, randomizedTeams));
            CreateOtherRounds(tournament, rounds);
            await SaveRoundsAsync(tournament);
        }

        private async Task SaveRoundsAsync(Tournament tournament)
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

        private static void CreateOtherRounds(Tournament tournament, int rounds)
        {
            var round = 2;
            List<Matchup> previousRound = tournament.RoundsNm[0];
            List<Matchup> currRound = new();
            Matchup currMatchup = new();

            while (round <= rounds)
            {
                foreach (Matchup matchup in previousRound)
                {
                    currMatchup.Entries.Add(new MatchupEntry { ParentMatchup = matchup });

                    if (currMatchup.Entries.Count > 1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new();
                    }
                }
                tournament.RoundsNm.Add(currRound);
                previousRound = currRound;
                currRound = new();
                round++;
            }
        }

        private static List<Matchup> CreateFirstRound(int byes, List<Team> teams)
        {
            List<Matchup> output = [];
            Matchup curr = new();

            foreach (var team in teams)
            {
                curr.Entries.Add(new MatchupEntry { TeamCompeting = team });

                if (byes > 0 || curr.Entries.Count > 1)
                {
                    curr.MatchupRound = 1;
                    output.Add(curr);
                    curr = new();

                    if (byes > 0)
                    {
                        byes--;
                    }
                }
            }
            return output;
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            var output = 0;
            var totalTeams = 1;

            for (var i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberOfTeams;
            return output;
        }

        private static int FindNumberOfRounds(List<Team> teams)
        {
            var output = 1;
            var val = 2;

            while (teams.Count > val)
            {
                output++;
                val *= 2;
            }
            return output;
        }

        private static List<Team> RandomizeTeamOrder(List<Team> teams)
        {
            return teams.OrderBy(t => Guid.NewGuid()).ToList();
        }
    }
}
