using Chirp.Core.Domain.Interfaces.Repositories;
using Chirp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Database.Repositories
{
    public sealed class ImageRepository : IImageRepository
    {
        private readonly ChirpContext _db;
        public ImageRepository(ChirpContext db) => _db = db;
        public Task<Image?> GetByIdAsync(Guid id)
            => _db.Images.FirstOrDefaultAsync(i => i.Id == id);
        public async Task AddAsync(Image image)
        => await _db.Images.AddAsync(image);

        public Task SaveChangesAsync()
            => _db.SaveChangesAsync();
    }
}
