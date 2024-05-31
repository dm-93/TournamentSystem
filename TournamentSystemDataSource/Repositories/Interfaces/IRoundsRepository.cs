using TournamentSystemModels;

namespace TournamentSystemDataSource.Repositories.Interfaces
{
    public interface IRoundsRepository
    {
        Task SaveRoundsAsync(Tournament tournament);
    }
}