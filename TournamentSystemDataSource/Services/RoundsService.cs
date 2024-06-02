using TournamentSystemDataSource.DTO.Pagination;
using TournamentSystemDataSource.DTO.Rounds;
using TournamentSystemDataSource.Repositories.Interfaces;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal sealed class RoundsService : IRoundsService
    {
        private readonly IRoundsRepository _roundsRepository;
        private readonly ITeamService _teamService;
        public RoundsService(IRoundsRepository roundsRepository, ITeamService teamService)
        {
            _roundsRepository = roundsRepository;
            _teamService = teamService;
        }

        public async Task<PaginationResponse<IEnumerable<Matchup>>> GetMatchupsAsync(Pagination<GetNextRoundDto> pagination, CancellationToken cancellationToken)
        {
            return await _roundsRepository.GetTournamentRoundsAsync(pagination, cancellationToken);
        }

        public async Task UpdateMatchAsync(UpdateRequest matchup, CancellationToken cancellationToken)
        {
            var winnerTeam = await _teamService.GetTeamByIdAsync(matchup.Winner, cancellationToken);
            var match = new Matchup
            {
                Id = matchup.Id,
                Winner = winnerTeam,
                UpdatedOn = DateTime.UtcNow,
                Entries = matchup.Entries,
            };

            await _roundsRepository.UpdateMatchupAsync(match, cancellationToken);
        }

        public async Task<IEnumerable<Matchup>> GetRoundAsync(int tournamentId, int nextRoundId, CancellationToken cancellationToken)
        {
            var tournRounds = await _roundsRepository.GetTournamentRoundsAsync(tournamentId, cancellationToken);
            return tournRounds.Where(x => x.MatchupRound == nextRoundId).ToList();
        }

        public async Task GenerateRoundAsync(int tournamentId, int prevRoundId, CancellationToken cancellationToken)
        {
            var tournRounds = await _roundsRepository.GetTournamentRoundsAsync(tournamentId, cancellationToken);
            var matchups = GetPreviousRound(tournRounds, prevRoundId);
            var winners = matchups.Select(x => x.Winner).ToList();
            if (!ValidateAllWinnersOfPrevRound(winners))
            {
                throw new Exception("Не все победители выявлены в предыдущем этапе.");
            }

            var randomizedWinners = RandomizeTeamOrder(winners);
            var stack = new Stack<Team>(randomizedWinners);

            foreach (var round in tournRounds.Where(x => x.MatchupRound == prevRoundId + 1))
            {
                if (round.Entries.All(x => x.TeamCompeting is not null))
                {
                    continue;
                }

                foreach (var entry in round.Entries)
                {
                    entry.TeamCompeting ??= stack.Pop();
                }

                await _roundsRepository.UpdateMatchupAsync(round, cancellationToken);
            }
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

        private bool ValidateAllWinnersOfPrevRound(List<Team> winners)
        {
            return winners.All(x => x is not null);
        }

        private IEnumerable<Matchup> GetPreviousRound(List<Matchup> tournamentMatchups,
                                                                             int previousRoundNumber)
        {
            var prevRound = tournamentMatchups.Where(x => x.MatchupRound == previousRoundNumber).ToList();

            if (!prevRound.Any())
            {
                throw new ArgumentException("Ни одного раунда соревнований не найдено.");
            }

            return prevRound;
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
