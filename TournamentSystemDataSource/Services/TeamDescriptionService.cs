using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal class TeamDescriptionService : ITeamDescriptionService
    {
        private readonly GeneralContext _context;
        private readonly ILogger<TeamDescriptionService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TeamDescriptionService(GeneralContext context, ILogger<TeamDescriptionService> logger, IUnitOfWork unitOfWork)
        {
            _context = context;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TeamDescription>> GetTeamDescriptionsAsync(CancellationToken cancellationToken)
        {
            var teamDescriptions = await _context.TeamsDescriptions.ToListAsync(cancellationToken);
            return teamDescriptions;
        }

        public async Task<TeamDescription> GetTeamDescriptionByIdAsync(int teamDescriptionId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving team description with ID: {teamDescriptionId}...");

            var teamDescription = await _context.TeamsDescriptions.FindAsync(teamDescriptionId);

            if (teamDescription == null)
            {
                _logger.LogWarning($"Team description with ID {teamDescriptionId} not found.");
                throw new ArgumentException($"Описание команды с Id {teamDescriptionId} не найдено.");
            }

            _logger.LogInformation($"Team description with ID {teamDescriptionId} retrieved successfully.");

            return teamDescription;
        }

        public async Task<TeamDescription> CreateTeamDescriptionAsync(TeamDescription teamDescription, CancellationToken cancellationToken)
        {
            if (teamDescription == null)
            {
                throw new ArgumentNullException($"{nameof(teamDescription)} не может быть равно null.");
            }

            _logger.LogInformation("Creating a new team description...");

            var res = await _context.TeamsDescriptions.AddAsync(teamDescription);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation("Team description created successfully.");

            return res.Entity;
        }

        public async Task<TeamDescription> UpdateTeamDescriptionAsync(TeamDescription updatedTeamDescription, CancellationToken cancellationToken)
        {
            if (updatedTeamDescription == null)
            {
                throw new ArgumentNullException($"{nameof(updatedTeamDescription)} не может быть равно null.");
            }

            _logger.LogInformation($"Updating team description with ID: {updatedTeamDescription.Id}...");

            var existingTeamDescription = await _context.TeamsDescriptions.FindAsync(updatedTeamDescription.Id);

            if (existingTeamDescription == null)
            {
                _logger.LogWarning($"Team description with ID {updatedTeamDescription.Id} not found.");
                throw new ArgumentException($"Описание команды с Id {updatedTeamDescription.Id} не найдено.");
            }

            _context.Entry(existingTeamDescription).CurrentValues.SetValues(updatedTeamDescription);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation($"Team description with ID {updatedTeamDescription.Id} updated successfully.");

            return existingTeamDescription;
        }

        public async Task DeleteTeamDescriptionAsync(int teamDescriptionId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Deleting team description with ID: {teamDescriptionId}...");

            var teamDescription = await _context.TeamsDescriptions.FindAsync(teamDescriptionId);

            if (teamDescription == null)
            {
                _logger.LogWarning($"Team description with ID {teamDescriptionId} not found.");
                throw new ArgumentException($"Описание команды с Id {teamDescriptionId} не найдено.");
            }

            _context.TeamsDescriptions.Remove(teamDescription);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation($"Team description with ID {teamDescriptionId} deleted successfully.");
        }
    }
}