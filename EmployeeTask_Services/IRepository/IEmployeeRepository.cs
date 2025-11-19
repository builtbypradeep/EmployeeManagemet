using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeTask.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace EmployeeTask_Services.IRepository
{
    #region Interface
    public interface IEmployeeRepository 
    {
        Task<IEnumerable<EmployeeMaster>> GetAllEmpAsync();
        Task<EmployeeMaster?> GetbyIdAsync(int id);
        Task<int> AddAsync (EmployeeMaster obj);
        Task UpdateAsync (EmployeeMaster obj);
        Task DeleteAsync (int id);    
    }
    #endregion
}
