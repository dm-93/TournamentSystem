namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveAsync(CancellationToken cancellationToken);
    }
}