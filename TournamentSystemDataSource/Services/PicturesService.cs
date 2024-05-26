using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

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

        public async Task<Pictures?> UpdatePictureAsync(int id, string pictureBase64, CancellationToken cancellationToken)
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

        private async Task<Pictures> ProcessPictureAsync(string pictureBase64, CancellationToken cancellationToken)
        {
            string filePath = string.Empty;
            if (!string.IsNullOrEmpty(pictureBase64))
            {
                var base64Data = Regex.Match(pictureBase64, @"data:image/(?<type>.+?);base64,(?<data>.+)").Groups["data"].Value;
                var bytes = Convert.FromBase64String(base64Data);
                var fileName = $"{Guid.NewGuid()}.png";
                filePath = Path.Combine(Environment.CurrentDirectory, fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, bytes, cancellationToken);
            }

            return new Pictures { PictureUrl = filePath };
        }
    }
}
