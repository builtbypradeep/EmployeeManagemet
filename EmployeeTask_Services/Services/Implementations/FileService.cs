using EmployeeTask_Services.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
namespace EmployeeTask.Services.Implementations
{
    public class FileService : IFileService
    {
        #region Properties
        private readonly string _rootPath;

        #endregion

        #region Constructor
        public FileService(string rootpath)
        {
            _rootPath = rootpath;
        }
        #endregion

        #region Delete Image
        public void DeleteImage(string ImagePath)
        {
            if (string.IsNullOrEmpty(ImagePath))
                return;

            string fileName = Path.GetFileName(ImagePath);

            string folder = Path.Combine(_rootPath, "Uploads", "Employees");

            string fullPath = Path.Combine(folder, fileName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
        #endregion

        #region Update Image
        public async Task<string> UploadImageAsync(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
                return null;

            string folder = Path.Combine(_rootPath, "Uploads", "Employees");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = $"{Guid.NewGuid()}_{formFile.FileName}";

            string fullPath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return $"/Uploads/Employees/{fileName}";
        }

        #endregion

    }
}
