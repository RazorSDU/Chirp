using Chirp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Core.Domain.Interfaces.Services
{
    public interface IImageService
    {
        Task<Image?> GetByIdAsync(Guid id);
        Task<Image> CreateAsync(byte[] data, string filename, string contentType);
    }
}
