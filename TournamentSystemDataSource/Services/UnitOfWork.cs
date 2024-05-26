using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly GeneralContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(GeneralContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            UpdateDateChange(_context);
            if (await _context.SaveChangesAsync(cancellationToken) <= 0)
            {
                _logger.LogError("Failed to save changes to the database.");
                throw new InvalidOperationException("Не удалось сохранить изменения в базе данных.");
            }
        }

        private static void UpdateDateChange(DbContext context)
        {
            var entries = context.ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                entry.Entity.UpdatedOn = DateTime.Now;
            }
        }
    }
}
