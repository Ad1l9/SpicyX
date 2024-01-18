namespace SpicyXWebsite.Utilities.Extension
{
    public static class FileValidation
    {
        public static bool ValidateType(this IFormFile file, string type = "image/")
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }

        public static bool ValidateSize(this IFormFile file, int limitMb = 2)
        {
            if (file.Length <= limitMb * 1024 * 1024)
            {
                return true;
            }
            return false;
        }

        public static string CreatePath(string root, string fileName, params string[] folders)
        {
            string path = root;
            //for (int i = 0; i < folders.Length; i++)
            //{
            //    path = Path.Combine(path, folders[i]);
            //}
            foreach (string item in folders)
            {
                path = Path.Combine(path, item);
            }
            path = Path.Combine(path, fileName);
            return path;
        }
        public async static Task<string> CreateFile(this IFormFile file, string root, params string[] folders)
        {
            string fileExtension = Path.GetExtension(file.FileName);
            string fileName = $"{Guid.NewGuid}{fileExtension}";

            string path = CreatePath(root, fileName, folders);
            using (var fs = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
            return fileName;
        }
        public static void DeleteFile(this string fileName, string root, params string[] folders)
        {
            string path = CreatePath(root, fileName, folders);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
