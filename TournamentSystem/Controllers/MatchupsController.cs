using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> CreateMatchups([FromBody] int tournamentId, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentService.GetByIdAsync(tournamentId, cancellationToken);
            if (tournament == null) 
            { 
                return BadRequest(); 
            }

            await _roundsService.CreateRoundsAsync(tournament, cancellationToken);
            return Ok();
        }
    }
}
