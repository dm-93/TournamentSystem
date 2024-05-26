using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    public interface IPicturesService
    {
        Task<Pictures?> CreatePictureAsync(string pictureBase64, CancellationToken cancellationToken);
        Task<Pictures?> GetPictureAsync(int id, CancellationToken cancellationToken);
        Task<Pictures?> UpdatePictureAsync(int id, string pictureBase64, CancellationToken cancellationToken);
    }
}