using TournamentSystemDataSource.DTO.Pagination;
using TournamentSystemDataSource.Repositories.Interfaces;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal sealed class RoundsService : IRoundsService
    {
        private readonly IRoundsRepository _roundsRepository;
        public RoundsService(IRoundsRepository roundsRepository)
        {
            _roundsRepository = roundsRepository;
        }

        public async Task<PaginationResponse<IEnumerable<Matchup>>> GetMatchupsAsync(Pagination<int> pagination, CancellationToken cancellationToken)
        {
            return await _roundsRepository.GetTournamentRoundsAsync(pagination, cancellationToken);
        }

        public async Task CreateRoundsAsync(Tournament tournament, CancellationToken cancellationToken)
        {
            var randomizedTeams = RandomizeTeamOrder(tournament.EnteredTeams);
            var rounds = FindNumberOfRounds(randomizedTeams);
            var byes = NumberOfByes(rounds, randomizedTeams.Count);
            tournament.RoundsNm.Add(CreateFirstRound(byes, randomizedTeams));
            CreateOtherRounds(tournament, rounds);
            await _roundsRepository.SaveRoundsAsync(tournament);
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

        //TODO: Check this logic again
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
