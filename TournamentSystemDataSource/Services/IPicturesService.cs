using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    public interface IPicturesService
    {
        Task<string> ConvertPictureToBase64String(int id, CancellationToken cancellationToken);
        Task<Pictures?> CreatePictureAsync(string pictureBase64, CancellationToken cancellationToken);
        Task<Pictures?> GetPictureAsync(int id, CancellationToken cancellationToken);
        Task<string> ResizeImageToBase64Async(string imagePath, CancellationToken cancellationToken, int width = 300, int height = 300);
        Task<Pictures?> UpdatePictureAsync(int id, string pictureBase64, CancellationToken cancellationToken);
    }
}