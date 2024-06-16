using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TournamentSystemDataSource.DTO;
using TournamentSystemDataSource.DTO.Pagination;
using TournamentSystemDataSource.DTO.Person.Request;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels.Identity;

namespace TournamentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _service;
        private readonly UserManager<User> _userManager;

        public PersonsController(IPersonService service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination<int> pagination, CancellationToken cancellationToken)
        {
            var res = await _service.GetPersonsPaginatedAsync(pagination, cancellationToken);
            return res is not null ? Ok(res) : NotFound();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] GetByConditionRequest request, CancellationToken cancellationToken)
        {
            var res = await _service.GetByConditionAsync(request, cancellationToken);
            return res is not null ? Ok(res) : NotFound();
        }

        [HttpGet("getByEmail")]
        public async Task<IActionResult> Get([FromQuery] string email, CancellationToken cancellationToken)
        {
            email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            var res = await _userManager.FindByEmailAsync(email);
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
