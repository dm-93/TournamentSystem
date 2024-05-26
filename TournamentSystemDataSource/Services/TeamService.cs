using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TournamentSystemDataSource.DTO;
using TournamentSystemDataSource.Extensions;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal class TeamService : ITeamService
    {
        private readonly GeneralContext _context;
        private readonly ILogger<TeamService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TeamService(GeneralContext context, ILogger<TeamService> logger, IUnitOfWork unitOfWork)
        {
            _context = context;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving teams...");

            var teams = await _context.Teams
                .Include(t => t.Description)
                .Include(t => t.TeamMembers)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Teams retrieved successfully.");

            return teams;
        }

        public async Task<IEnumerable<Team>> GetTeamsByConditionAsync(GetByConditionRequest request, CancellationToken cancellationToken)
        {
            var property = typeof(Team).GetProperty(request.PropertyName,
                                                          BindingFlags.Public |
                                                          BindingFlags.Instance |
                                                          BindingFlags.IgnoreCase |
                                                          BindingFlags.FlattenHierarchy)
                ?? throw new ArgumentNullException($"Не получилось найти св-во: {request.PropertyName} в {nameof(Team)}.");
            var teams = await _context.Teams
                .Include(t => t.TeamMembers)
                .Include(t => t.Description)
                .ApplyFilter(property, request.PropertyValue, request.PropertyValueType)
                .Where(t => !t.Deleted)
                .ToListAsync(cancellationToken) ?? Enumerable.Empty<Team>();

            if (!teams.Any())
            {
                _logger.LogWarning($"Team with {property.Name} = {request.PropertyValue} not found.");
                throw new ArgumentException($"Турнир с Id {property.Name} = {request.PropertyValue} не найден.");
            }

            return teams;
        }

        public async Task<Team> GetTeamByIdAsync(int teamId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving team with ID: {teamId}...");

            var team = await _context.Teams
                .Include(t => t.Description)
                .Include(t => t.TeamMembers)
                .FirstOrDefaultAsync(t => t.Id == teamId, cancellationToken);

            if (team == null)
            {
                _logger.LogWarning($"Team with ID {teamId} not found.");
                throw new ArgumentException($"Команда с Id {teamId} не найдена.");
            }

            _logger.LogInformation($"Team with ID {teamId} retrieved successfully.");
            return team;
        }

        public async Task<Team> CreateTeamAsync(Team team, CancellationToken cancellationToken)
        {
            if (team == null)
            {
                throw new ArgumentNullException($"{nameof(team)} не может быть равно null.");
            }

            _logger.LogInformation("Creating a new team...");
            team.CreatedOn = DateTime.UtcNow;
            var res = await _context.Teams.AddAsync(team);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation("Team created successfully.");
            return res.Entity;
        }

        public async Task<Team> UpdateTeamAsync(Team updatedTeam, CancellationToken cancellationToken)
        {
            if (updatedTeam == null)
            {
                throw new ArgumentNullException($"{nameof(updatedTeam)} не может быть равно null.");
            }

            _logger.LogInformation($"Updating team with ID: {updatedTeam.Id}...");

            var existingTeam = await _context.Teams.Include(x => x.Description)
                                                   .FirstOrDefaultAsync(x => x.Id == updatedTeam.Id);

            if (existingTeam == null)
            {
                _logger.LogWarning($"Team with ID {updatedTeam.Id} not found.");
                throw new ArgumentException($"Команда с Id {updatedTeam.Id} не найдена.");
            }

            _context.Entry(existingTeam).CurrentValues.SetValues(updatedTeam);

            if (updatedTeam.Description != null)
            {
                if (existingTeam.Description == null)
                {
                    existingTeam.Description = updatedTeam.Description;
                }
                else
                {
                    _context.Entry(existingTeam.Description).CurrentValues.SetValues(updatedTeam.Description);
                }
            }

            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"Team with ID {updatedTeam.Id} updated successfully.");
            return existingTeam;
        }

        public async Task DeleteTeamAsync(int teamId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Deleting team with ID: {teamId}...");

            var team = await _context.Teams.FindAsync(teamId);

            if (team == null)
            {
                _logger.LogWarning($"Team with ID {teamId} not found.");
                throw new ArgumentException($"Команда с Id {teamId} не найдена.");
            }

            _context.Teams.Remove(team);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"Team with ID {teamId} deleted successfully.");
        }
    }
}