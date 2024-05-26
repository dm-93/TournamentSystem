using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal class PrizeService : IPrizeService
    {
        private readonly GeneralContext _context;
        private readonly ILogger<PrizeService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public PrizeService(GeneralContext context, ILogger<PrizeService> logger, IUnitOfWork unitOfWork)
        {
            _context = context;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Prize>> GetPrizesAsync(CancellationToken cancellationToken)
        {
            var prizes = await _context.Prizes.ToListAsync(cancellationToken);
            return prizes;
        }

        public async Task<Prize> GetPrizeByIdAsync(int prizeId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving prize with ID: {prizeId}...");

            var prize = await _context.Prizes.FindAsync(prizeId);

            if (prize == null)
            {
                _logger.LogWarning($"Prize with ID {prizeId} not found.");
                throw new ArgumentException($"Приз с Id {prizeId} не найден.");
            }

            _logger.LogInformation($"Prize with ID {prizeId} retrieved successfully.");

            return prize;
        }

        public async Task<Prize> CreatePrizeAsync(Prize prize, CancellationToken cancellationToken)
        {
            if (prize == null)
            {
                throw new ArgumentNullException($"{nameof(prize)} не может быть равен null.");
            }

            _logger.LogInformation("Creating a new prize...");

            var res = await _context.Prizes.AddAsync(prize);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation("Prize created successfully.");

            return res.Entity;
        }

        public async Task<Prize> UpdatePrizeAsync(Prize updatedPrize, CancellationToken cancellationToken)
        {
            if (updatedPrize == null)
            {
                throw new ArgumentNullException($"{nameof(updatedPrize)} не может быть равен null.");
            }

            _logger.LogInformation($"Updating prize with ID: {updatedPrize.Id}...");

            var existingPrize = await _context.Prizes.FindAsync(updatedPrize.Id);

            if (existingPrize == null)
            {
                _logger.LogWarning($"Prize with ID {updatedPrize.Id} not found.");
                throw new ArgumentException($"Приз с Id {updatedPrize.Id} не найден.");
            }

            _context.Entry(existingPrize).CurrentValues.SetValues(updatedPrize);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation($"Prize with ID {updatedPrize.Id} updated successfully.");

            return existingPrize;
        }

        public async Task DeletePrizeAsync(int prizeId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Deleting prize with ID: {prizeId}...");

            var prize = await _context.Prizes.FindAsync(prizeId);

            if (prize == null)
            {
                _logger.LogWarning($"Prize with ID {prizeId} not found.");
                throw new ArgumentException($"Приз с Id {prizeId} не найден.");
            }

            _context.Prizes.Remove(prize);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation($"Prize with ID {prizeId} deleted successfully.");
        }

        private async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}