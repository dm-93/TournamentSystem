using TournamentSystemModels;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface IRoundsService
    {
        Task CreateRoundsAsync(Tournament tournament, CancellationToken cancellationToken);
    }
}