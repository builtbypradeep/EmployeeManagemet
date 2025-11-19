

using EmployeeTask.Data;
using EmployeeTask.Models;
using EmployeeTask_Services.Services.Interfaces;

namespace EmployeeTask.Services.Implementations
{
    public class EmployeeValidator : IEmployeeValidator
    {
        #region Properties
        private readonly ApplicationDbContext _db;
        #endregion

        #region Constructor
        public EmployeeValidator(ApplicationDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Interface Implementation for Validations
        public async Task<(bool IsValid, string Message)> ValidateEmpAsync(EmployeeMaster obj)
        {
            if (obj.DateOfBirth >= DateTime.Today)
            {
                return (false, "Date of birth must be less than today.");
            }

            if (obj.DateOfJoinee >= DateTime.Today)
            {
                return (false, "Date of joinee must be less than or equal to today.");
            }

            if (obj.DateOfJoinee < obj.DateOfBirth)
            {
                return (false, "Date of joinee must be more than Date of Birth.");
            }
            
            if (_db.EmployeeMasters.Any(x => x.Email == obj.Email && x.Id != obj.Id))
            {
                return (false, "Email is already taken.");
            }

            if (_db.EmployeeMasters.Any(x => x.MobileNumber == obj.MobileNumber && x.Id != obj.Id))
            {
                return (false, "Mobile number already in use.");
            }

            if (_db.EmployeeMasters.Any(x => x.PanNumber == obj.PanNumber && x.Id != obj.Id))
            {
                return (false, "Pan number already in use.");
            }

            if (_db.EmployeeMasters.Any(x => x.PassportNumber == obj.PassportNumber && x.Id != obj.Id))
            {
                return (false, "Passport number already in use.");
            }

            return (true, "");
        }

        #endregion
    }
}
