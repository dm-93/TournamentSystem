using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TournamentSystemDataSource.DTO;
using TournamentSystemDataSource.Services.Interfaces;

namespace TournamentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _service;
        private readonly ITournamentStatisticsService _tournamentStatisticsService;
        public TournamentController(ITournamentService service, ITournamentStatisticsService tournamentStatisticsService)
        {
            _service = service;
            _tournamentStatisticsService = tournamentStatisticsService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

            if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            if (userRole.Equals("user", StringComparison.OrdinalIgnoreCase))
            {
                _service.GetTournamentsByUserEmailAsync(email, cancellationToken);
            }

            var res = await _service.GetTournamentsAsync(cancellationToken);
            return res is not null ? Ok(res) : NotFound();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] GetByConditionRequest request, CancellationToken cancellationToken)
        {
            var res = await _service.GetTournamentByConditionAsync(request, cancellationToken);
            return res is not null ? Ok(res) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] TournamentDto tournament, CancellationToken cancellationToken)
        {
            var res = await _service.CreateTournamentAsync(tournament, cancellationToken);
            return res is not null ? Created(string.Empty, res) : BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] TournamentDto tournamentDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _service.UpdateTournamentAsync(tournamentDto, cancellationToken);
            return res != null ? Ok(res) : BadRequest();
        }

        [HttpPatch("{id}/complete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CompleteTournament(int id, CancellationToken cancellationToken)
        {
            await _service.CompleteTournamentAsync(id, cancellationToken);
            return Ok();
        }


        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task Delete([FromQuery] int tournamentId, CancellationToken cancellationToken)
        {
            await _service.DeleteTournamentAsync(tournamentId, cancellationToken);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> Statistics([FromQuery] int tournamentId, CancellationToken cancellationToken)
        {
            var result = await _tournamentStatisticsService.GetTrounamentStatistics(tournamentId, cancellationToken);
            return result is not null ? Ok(result) : BadRequest();
        }
    }
}
