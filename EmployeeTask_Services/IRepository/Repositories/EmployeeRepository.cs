using EmployeeTask.Data;
using EmployeeTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTask_Services.IRepository.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        #region Properties
        private readonly ApplicationDbContext _db;
        private readonly ILogger<EmployeeRepository> _logger;
        #endregion

        #region Constructor
        public EmployeeRepository(ApplicationDbContext db, ILogger<EmployeeRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        #endregion

        #region Add Employee
        public async Task<int> AddAsync(EmployeeMaster obj)
        {
            try
            {
                obj.PanNumber = obj.PanNumber.ToUpperInvariant();

                obj.PassportNumber = obj.PassportNumber.ToUpperInvariant();

                _db.EmployeeMasters.Add(obj);

                await _db.SaveChangesAsync();
                return obj.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while adding record. Ex: " + ex.Message);
                throw;
            }
        }
        #endregion

        #region Delete Employee
        public async Task DeleteAsync(int id)
        {
            try
            {
                var empRec = await _db.EmployeeMasters.FindAsync(id);

                if (empRec != null)
                {
                    empRec.IsDeleted = true;
                    empRec.IsActive = false;
                    empRec.DeletedDate = DateTime.Now;
                    await _db.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error while deleting recoord. Ex: {ex.Message}");
                throw;
            }
            
        }
        #endregion

        #region Get All Employees
        public async Task<IEnumerable<EmployeeMaster>> GetAllEmpAsync()
        {
            try
            {
                var val = _db.EmployeeMasters.Include(x => x.Country).Include(x => x.State)
               .Include(x => x.City).Where(x => !x.IsDeleted);

                return await val.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error while getting data from database. Ex: {ex.Message}");
                throw;
            }     
        }
        #endregion

        #region Get Employee by Id
        public async Task<EmployeeMaster?> GetbyIdAsync(int id)
        {
            try
            {
                return await _db.EmployeeMasters.Include(x => x.Country).Include(x => x.State).Include(x => x.City).FirstAsync(x => x.Id == id);
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error while getting data from database for the user id: {id}. Ex: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region Update Employee
        public async Task UpdateAsync(EmployeeMaster obj)
        {
            try
            {
                obj.PanNumber = obj.PanNumber.ToUpperInvariant();

                obj.PassportNumber = obj.PassportNumber.ToUpperInvariant();

                _db.Update(obj);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating data user id: {obj.Id}. Ex: {ex.Message}");
            }
        }
        #endregion
    }
}
