using Microsoft.AspNetCore.Mvc;
using TournamentSystemDataSource.DTO;
using TournamentSystemDataSource.DTO.Person.Request;
using TournamentSystemDataSource.Services.Interfaces;

namespace TournamentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _service;
        public PersonsController(IPersonService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination, CancellationToken cancellationToken)
        {
            var res = await _service.GetPersonsPaginatedAsync(pagination, cancellationToken);
            return res is not null ? Ok(res) : NotFound();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] GetPersonByConditionRequest request, CancellationToken cancellationToken)
        {
            var res = await _service.GetByConditionAsync(request, cancellationToken);
            return res is not null ? Ok(res) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonRequest person, CancellationToken cancellationToken)
        {
            var res = await _service.CreateAsync(person, cancellationToken);
            return res is not null ? Ok(res) : BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePersonRequest person, CancellationToken cancellationToken)
        {
            var res = await _service.UpdateAsync(person, cancellationToken);
            return res is not null ? Ok(res) : BadRequest();
        }

        [HttpDelete]
        public async Task Delete([FromQuery] int personId, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(personId, cancellationToken);
        }
    }
}
