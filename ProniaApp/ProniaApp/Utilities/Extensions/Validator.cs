using ProniaApp.Utilities.Enums;

namespace ProniaApp.Utilities.Extensions
{
    public static class Validator
    {
        public static bool ValidateType(this IFormFile formFile, string type)
        {
            if (formFile.ContentType.Contains(type))
            {
                return true;
            }

            return false;
        }

        public static bool ValidateSize(this IFormFile formFile, FileSize fileSize, int size)
        {
            switch (fileSize)
            {
                case FileSize.KB:
                    return formFile.Length > size * 1024;
                case FileSize.MB:
                    return formFile.Length > size * 1024 * 1024;
                case FileSize.GB:
                    return formFile.Length > size * 1024 * 1024 * 1024;
            }

            return false;
        }



        public async static Task<string> CreateFile(this IFormFile formFile, params string[] roots)
        {
            string fileName = String.Concat(Guid.NewGuid().ToString(), formFile.FileName);


            // /Users/gulshanzalova/Desktop/MVCProjects-APA/ProniaApp/wwwroot/assets/images/website-images/7a655849-05b1-48d0-83c7-5cbdf240336amy_flower.jpeg

            string path = string.Empty;

            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }

            path = Path.Combine(path, fileName);

            using (FileStream fileStream = new(path, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            };


            return fileName;

        }


        public static void DeleteFile(this string filename, params string[] roots)
        {
            string path = string.Empty;

            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }

            path = Path.Combine(path, filename);


            File.Delete(path);
        }
    }
}
