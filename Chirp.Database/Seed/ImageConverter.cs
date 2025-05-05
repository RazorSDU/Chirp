// Chirp.Database/Seed/ImageConverter.cs
using System.IO;

namespace Chirp.Database.Seed
{
    /// <summary>
    /// Helper for turning image files on disk into byte[] blobs.
    /// </summary>
    public static class ImageConverter
    {
        public static byte[] ConvertToBytes(string filePath)
        {
            // ensure placeholder.png is copied to output directory (CopyToOutputDirectory in csproj)
            return File.ReadAllBytes(filePath);
        }
    }
}
