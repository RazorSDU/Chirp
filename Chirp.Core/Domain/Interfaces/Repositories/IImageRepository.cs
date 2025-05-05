using System;
using Chirp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Core.Domain.Interfaces.Repositories
{
    public interface IImageRepository
    {
        Task<Image?> GetByIdAsync(Guid id);
        Task AddAsync(Image image);
        Task SaveChangesAsync();
    }
}
