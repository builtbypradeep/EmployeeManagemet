using EmployeeTask.Models;

namespace EmployeeTask_Services.Services.Interfaces
{
    #region Interface 
    public interface IEmployeeValidator
    {
        Task<(bool IsValid, string Message)> ValidateEmpAsync(EmployeeMaster obj);
    }

    #endregion
}
