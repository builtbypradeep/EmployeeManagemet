
using Azure.Core;
using EmployeeTask.Data;
using EmployeeTask.Models;
using EmployeeTask_Services.IRepository;
using EmployeeTask_Services.Services.Interfaces;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Nodes;

namespace EmployeeTask.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        #region Properties
        private readonly IEmployeeRepository _emprepo;

        private readonly IFileService _fileService;

        private readonly ApplicationDbContext _db;

        #endregion

        #region Constructor
        public EmployeeService(IEmployeeRepository emprepo, IFileService fileService, ApplicationDbContext db)
        {
            _emprepo = emprepo;
            _fileService = fileService;
            _db = db;
        }

        #endregion

        #region Add Employee
        public async Task AddEmployeeAsync(EmployeeMaster employee)
        {
           employee.EmployeeCode = (_db.EmployeeMasters.Count() + 1).ToString("D3");
            employee.CreatedDate = DateTime.Now;    

            employee.IsDeleted = false;

            if (employee.ImagePath != null)
                employee.ProfileImage = await _fileService.UploadImageAsync(employee.ImagePath);

            await _emprepo.AddAsync(employee);
        }

        #endregion

        #region Update Employee
        public async Task UpdateEmployeeAsync(EmployeeMaster employee)
        {

           var data = await _emprepo.GetbyIdAsync(employee.Id);

            if (data is null)
                throw new ArgumentException("Employee not found.");

            data.FirstName = employee.FirstName;
            data.LastName = employee.LastName;
            data.Email = employee.Email;
            data.MobileNumber   = employee.MobileNumber;
            data.PanNumber = employee.PanNumber;
            data.PassportNumber = employee.PassportNumber;
            data.DateOfBirth = employee.DateOfBirth;
            data.DateOfJoinee = employee.DateOfJoinee;
            data.CountryID = employee.CountryID;
            data.StateID = employee.StateID;
            data.CityID = employee.CityID;
            data.Gender = employee.Gender;
            data.IsActive = employee.IsActive;
            data.UpdatedDate = DateTime.Now;

            
            if (employee.IsImageDeleted == true || employee.ImagePath != null)
            {
                _fileService.DeleteImage(data.ProfileImage);
                data.ProfileImage = await _fileService.UploadImageAsync(employee.ImagePath);

            }

            await _emprepo.UpdateAsync(data);

        }
        #endregion
    }
}
