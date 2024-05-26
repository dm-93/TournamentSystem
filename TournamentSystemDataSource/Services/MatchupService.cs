using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal sealed class MatchupService
    {
        private readonly GeneralContext _context;
        private readonly ILogger<MatchupService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public MatchupService(GeneralContext context = null, ILogger<MatchupService> logger = null, IUnitOfWork unitOfWork = null)
        {
            _context = context;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Matchup>> GetMatchupsAsync(CancellationToken cancellationToken)
        {
            return await _context.Matchups.Include(x => x.Entries).Include(m => m.Winner).ToListAsync(cancellationToken);
        }
    }
}
