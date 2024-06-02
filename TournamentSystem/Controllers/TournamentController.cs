using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentSystemDataSource.DTO;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _service;
        public TournamentController(ITournamentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
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
    }
}
