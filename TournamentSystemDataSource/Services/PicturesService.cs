using SixLabors.ImageSharp.Formats.Png;
using System.Text.RegularExpressions;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using TournamentSystemDataSource.Contexts;

namespace TournamentSystemDataSource.Services
{
    internal sealed class PicturesService : IPicturesService
    {
        private readonly GeneralContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public PicturesService(GeneralContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Pictures?> GetPictureAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Pictures.FindAsync(id, cancellationToken);
        }

        public async Task<Pictures?> UpsertPictureAsync(int id, string pictureBase64, CancellationToken cancellationToken)
        {
            var picture = await GetPictureAsync(id, cancellationToken);

            if (picture is null)
            {
                return await CreatePictureAsync(pictureBase64, cancellationToken);
            }

            picture.PictureUrl = (await ProcessPictureAsync(pictureBase64, cancellationToken)).PictureUrl;
            await _unitOfWork.SaveAsync(cancellationToken);
            return picture;
        }

        public async Task<Pictures?> CreatePictureAsync(string pictureBase64, CancellationToken cancellationToken)
        {
            var picture = await ProcessPictureAsync(pictureBase64, cancellationToken);

            if (string.IsNullOrEmpty(picture.PictureUrl))
                return null;

            picture.CreatedOn = DateTime.Now;
            var addedPicture = await _context.Pictures.AddAsync(picture, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            return addedPicture.Entity;
        }

        public async Task<string> ConvertPictureToBase64String(int id, CancellationToken cancellationToken)
        {
            var picture = await GetPictureAsync(id, cancellationToken);

            if (picture is null || string.IsNullOrEmpty(picture.PictureUrl) || !File.Exists(picture.PictureUrl))
                return string.Empty;

            byte[] imageBytes = await File.ReadAllBytesAsync(picture.PictureUrl, cancellationToken);
            string base64String = Convert.ToBase64String(imageBytes);
            string mimeType = GetMimeType(picture.PictureUrl);

            return $"data:{mimeType};base64,{base64String}";
        }

        public async Task<string> ResizeImageToBase64Async(string imagePath, CancellationToken cancellationToken,int width = 300, int height = 300)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                return string.Empty;

            using var image = await Image.LoadAsync(imagePath, cancellationToken);
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Max
            }));

            using var ms = new MemoryStream();
            image.Save(ms, new PngEncoder());
            var imageBytes = ms.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);
            string mimeType = GetMimeType(imagePath);

            return $"data:{mimeType};base64,{base64String}";
        }

        private async Task<Pictures> ProcessPictureAsync(string pictureBase64, CancellationToken cancellationToken)
        {
            string filePath = string.Empty;
            if (!string.IsNullOrEmpty(pictureBase64))
            {
                var base64Data = Regex.Match(pictureBase64, @"data:image/(?<type>.+?);base64,(?<data>.+)").Groups["data"].Value;
                var bytes = Convert.FromBase64String(base64Data);
                var fileName = $"{Guid.NewGuid()}.png";
                filePath = Path.Combine($"{Environment.CurrentDirectory}\\Pictures", fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, bytes, cancellationToken);
            }

            return new Pictures { PictureUrl = filePath };
        }

        private static string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }
    }
}
