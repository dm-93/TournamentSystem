using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentSystemDataSource.DTO.Admin;
using TournamentSystemDataSource.DTO.Person.Request;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels.Identity;

namespace TournamentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPersonService _personService;

        public AdminController(UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager,
            IPersonService personService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _personService = personService;
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddNewUser(AdminAddUserDto userDto, CancellationToken cancellationToken)
        {
            var user = new User { UserName = userDto.Name, Email = userDto.Email };
            var res = await _userManager.CreateAsync(user, userDto.Password);

            if (!res.Succeeded) 
            { 
                return BadRequest();
            }

            var personCreated = await _personService.CreateAsync(new CreatePersonRequest(userDto), cancellationToken);

            return res.Succeeded && personCreated is not null ? Ok(userDto) : BadRequest();
        }

        [HttpGet]
        public async Task<IEnumerable<GetUsersResponse>> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.AsNoTracking()
                                                .ToListAsync(cancellationToken);

            var res = new List<GetUsersResponse>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var a = new GetUsersResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles
                };
                res.Add(a);
            }
            return res;
        }

        [HttpDelete]
        public async Task<IdentityResult> DeleteUser([FromQuery] string userId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return IdentityResult.Failed();
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return IdentityResult.Failed();
            }
            return await _userManager.DeleteAsync(user);
        }

        [HttpPost("removeRole")]
        public async Task<IdentityResult> RemoveUserRole([FromBody] RolesDto rolesDto, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(rolesDto.id);
            var role = await _roleManager.FindByNameAsync(rolesDto.roleName);
            var userAlreadyInRole = await _userManager.IsInRoleAsync(user, role.Name);

            if (user == null || !userAlreadyInRole)
            {
                return IdentityResult.Failed();
            }

            return await _userManager.RemoveFromRoleAsync(user, role.Name);
        }

        [HttpPost("addRole")]
        public async Task<IdentityResult> AssignUserRole([FromBody] RolesDto rolesDto, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(rolesDto.id);
            var role = await _roleManager.FindByNameAsync(rolesDto.roleName);
            var userAlreadyInRole = await _userManager.IsInRoleAsync(user, role.Name);

            if (user == null || userAlreadyInRole)
            {
                return IdentityResult.Failed();
            }

            return await _userManager.AddToRoleAsync(user, role.Name);
        }
    }

    public record RolesDto(string id, string roleName);
}
