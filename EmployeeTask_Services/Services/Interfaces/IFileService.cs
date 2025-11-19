using Microsoft.AspNetCore.Http;

namespace EmployeeTask_Services.Services.Interfaces
{
    #region Interface
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile formFile);
        void DeleteImage(string ImagePath);
    }
    #endregion
}
