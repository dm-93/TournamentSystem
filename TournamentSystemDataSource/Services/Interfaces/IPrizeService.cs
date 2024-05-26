using TournamentSystemModels;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface IPrizeService
    {
        Task<Prize> CreatePrizeAsync(Prize prize, CancellationToken cancellationToken);
        Task DeletePrizeAsync(int prizeId, CancellationToken cancellationToken);
        Task<Prize> GetPrizeByIdAsync(int prizeId, CancellationToken cancellationToken);
        Task<IEnumerable<Prize>> GetPrizesAsync(CancellationToken cancellationToken);
        Task<Prize> UpdatePrizeAsync(Prize updatedPrize, CancellationToken cancellationToken);
    }
}