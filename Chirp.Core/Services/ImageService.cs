using Chirp.Core.Domain.Interfaces.Services;
using Chirp.Core.Domain.Interfaces.Repositories;
using Chirp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Core.Services
{
    public sealed class ImageService : IImageService
    {
        private readonly IImageRepository _images;
        public ImageService(IImageRepository images) => _images = images;
        public Task<Image?> GetByIdAsync(Guid id) => _images.GetByIdAsync(id);
        public async Task<Image> CreateAsync(byte[] data, string filename, string contentType)
        {
            var img = new Image
            {
                Id = Guid.NewGuid(),
                Data = data,
                Filename = filename,
                ContentType = contentType,
                UploadedAt = DateTime.UtcNow
            };

            await _images.AddAsync(img);
            await _images.SaveChangesAsync();
            return img;
        }
    }
}
