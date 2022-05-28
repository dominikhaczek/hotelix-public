using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Hotelix.Offer.Utilities
{
    public class ImageManager
    {
        public static string SaveImage(IFormFile file, string path)
        {
            var uniqueFileName = GetUniqueFileName(file.FileName);
            var filePath = Path.Combine(path, uniqueFileName);
            file.CopyTo(new FileStream(filePath, FileMode.Create));

            var returnPath = filePath.Split("wwwroot");

            return returnPath[^1];
        }

        private static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }
    }
}
