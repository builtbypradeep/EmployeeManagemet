using EmployeeTask.Models;

namespace EmployeeTask_Services.Services.Interfaces
{
    #region Interface
    public interface IEmployeeService
    {
        Task AddEmployeeAsync(EmployeeMaster employee);

        Task UpdateEmployeeAsync(EmployeeMaster employee);
    }

    #endregion
}
