﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using TournamentSystemDataSource.DTO;
using TournamentSystemDataSource.Extensions;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal class TournamentService : ITournamentService
    {
        private readonly GeneralContext _context;
        private readonly ILogger<TournamentService> _logger;
        private readonly IPicturesService _picturesService;
        private readonly IUnitOfWork _unitOfWork;


        public TournamentService(GeneralContext context,
                                 ILogger<TournamentService> logger,
                                 IUnitOfWork unitOfWork,
                                 IPicturesService picturesService)
        {
            _context = context;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _picturesService = picturesService;
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving tournaments...");

            var tournaments = await _context.Tournaments.AsNoTracking()
                .Include(t => t.EnteredTeams)
                .Include(t => t.Prizes)
                .Include(t => t.Rounds)
                .Include(t => t.TournamentPicture)
                .AsSplitQuery()
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Tournaments retrieved successfully.");

            return tournaments;
        }

        public async Task<IEnumerable<Tournament>> GetTournamentByConditionAsync(GetByConditionRequest request, CancellationToken cancellationToken)
        {
            var property = typeof(Tournament).GetProperty(request.PropertyName,
                                                          BindingFlags.Public | 
                                                          BindingFlags.Instance | 
                                                          BindingFlags.IgnoreCase | 
                                                          BindingFlags.FlattenHierarchy)
                ?? throw new ArgumentNullException($"Не получилось найти св-во: {request.PropertyName} в {nameof(Tournament)}.");
            var tournament = await _context.Tournaments.AsNoTracking()
                .Include(t => t.EnteredTeams)
                .Include(t => t.Prizes)
                .Include(t => t.Rounds)
                .Include(t => t.TournamentPicture)
                .AsSplitQuery()
                .ApplyFilter(property, request.PropertyValue, request.PropertyValueType)
                .ToListAsync(cancellationToken);

            if (tournament == null)
            {
                _logger.LogWarning($"Tournament with {property.Name} = {request.PropertyValue} not found.");
                throw new ArgumentException($"Турнир с Id {property.Name} = {request.PropertyValue} не найден.");
            }

            _logger.LogInformation($"Tournament with ID {property.Name} = {request.PropertyValue} retrieved successfully.");

            return tournament;
        }

        public async Task<Tournament> CreateTournamentAsync(Tournament tournament, CancellationToken cancellationToken)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException($"{nameof(tournament)} не может быть равно null.");
            }

            _logger.LogInformation("Creating a new tournament...");
            tournament.CreatedOn = DateTime.UtcNow;
            var res = await _context.Tournaments.AddAsync(tournament);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation("Tournament created successfully.");
            return res.Entity;
        }

        public async Task<Tournament> UpdateTournamentAsync(TournamentDto updatedTournament, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(updatedTournament);

            _logger.LogInformation($"Updating tournament with ID: {updatedTournament.Id}...");

            var existingTournament = await _context.Tournaments.Include(t => t.TournamentPicture)
                                                               .FirstOrDefaultAsync(t => t.Id == updatedTournament.Id, cancellationToken);

            if (existingTournament == null)
            {
                _logger.LogWarning($"Tournament with ID {updatedTournament.Id} not found.");
                throw new ArgumentException($"Турнир с Id {updatedTournament.Id} не найден.");
            }

            if (existingTournament.TournamentPicture is not null)
            {
                existingTournament.TournamentPicture = await _picturesService.UpdatePictureAsync(existingTournament.TournamentPicture.Id,
                                                                                                 updatedTournament.TournamentPictureBase64,
                                                                                                 cancellationToken);
            }
            else if (existingTournament.TournamentPicture is null && !string.IsNullOrEmpty(updatedTournament.TournamentPictureBase64))
            {
                existingTournament.TournamentPicture = await _picturesService.CreatePictureAsync(updatedTournament.TournamentPictureBase64, cancellationToken);
            }

            _context.Entry(existingTournament).CurrentValues.SetValues(updatedTournament);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"Tournament with ID {updatedTournament.Id} updated successfully.");
            return existingTournament;
        }

        public async Task DeleteTournamentAsync(int tournamentId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Deleting tournament with ID: {tournamentId}...");

            var tournament = await _context.Tournaments.FindAsync(tournamentId);

            if (tournament == null)
            {
                _logger.LogWarning($"Tournament with ID {tournamentId} not found.");
                throw new ArgumentException($"Турнир с Id {tournamentId} не найден.");
            }

            _context.Tournaments.Remove(tournament);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"Tournament with ID {tournamentId} deleted successfully.");
        }
    }
}