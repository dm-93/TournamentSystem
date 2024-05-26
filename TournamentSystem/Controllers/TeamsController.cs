using Microsoft.AspNetCore.Mvc;
using TournamentSystemDataSource.DTO;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _service;
        public TeamsController(ITeamService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var res = await _service.GetTeamsAsync(cancellationToken);
            return res is not null ? Ok(res) : NotFound();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] GetByConditionRequest request, CancellationToken cancellationToken)
        {
            var res = await _service.GetTeamsByConditionAsync(request, cancellationToken);
            return res is not null ? Ok(res) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Team team, CancellationToken cancellationToken)
        {
            var res = await _service.CreateTeamAsync(team, cancellationToken);
            return res is not null ? Created(string.Empty, res) : BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Team team, CancellationToken cancellationToken)
        {
            var res = await _service.UpdateTeamAsync(team, cancellationToken);
            return res is not null ? Ok(res) : BadRequest();
        }

        [HttpDelete]
        public async Task Delete([FromQuery] int teamId, CancellationToken cancellationToken)
        {
            await _service.DeleteTeamAsync(teamId, cancellationToken);
        }
    }
}
