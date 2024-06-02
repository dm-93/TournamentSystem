using Microsoft.AspNetCore.Mvc;
using TournamentSystemDataSource.DTO.Pagination;
using TournamentSystemDataSource.DTO.Rounds;
using TournamentSystemDataSource.Services.Interfaces;

namespace TournamentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchupsController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;
        private readonly IRoundsService _roundsService;
        public MatchupsController(ITournamentService tournamentService, IRoundsService roundsService)
        {
            _tournamentService = tournamentService;
            _roundsService = roundsService;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMatchAsync([FromBody] UpdateRequest matchup, CancellationToken cancellationToken)
        {
            await _roundsService.UpdateMatchAsync(matchup, cancellationToken);
            return Ok();
        }

        [HttpPost("nextRound")]
        public async Task<IActionResult> GetNextRoundAsync([FromBody] GetNextRoundDto getNextRoundDto, CancellationToken cancellationToken)
        {
            await _roundsService.GenerateRoundAsync(getNextRoundDto.tournamentId, getNextRoundDto.roundId, cancellationToken);
            var round = await _roundsService.GetRoundAsync(getNextRoundDto.tournamentId, getNextRoundDto.roundId + 1, cancellationToken);
            return Ok(round);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatchups([FromQuery] int tournamentId, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentService.GetByIdAsync(tournamentId, cancellationToken);
            if (tournament == null)
            {
                return BadRequest();
            }

            await _roundsService.CreateRoundsAsync(tournament, cancellationToken);
            return Ok();
        }

        [HttpPost("Get")]
        public async Task<IActionResult> GetMatchups([FromBody] Pagination<GetNextRoundDto> pagination, CancellationToken cancellationToken)
        {
            var res = await _roundsService.GetMatchupsAsync(pagination, cancellationToken);
            return res is not null ? Ok(res) : BadRequest();
        }
    }
}
