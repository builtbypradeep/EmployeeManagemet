using EmployeeTask.Data;
using EmployeeTask.Models;
using EmployeeTask_Services.IRepository;
using EmployeeTask_Services.IRepository.Repositories;
using EmployeeTask_Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTask.Controllers
{
    public class EmployeeController : Controller
    {
        #region Properties
        private readonly IEmployeeRepository _emp;
        private readonly IWebHostEnvironment _env;

        private readonly ApplicationDbContext _db;

        private readonly ILogger<EmployeeController> _logger;

        private readonly IEmployeeValidator _employeeValidator;

        private readonly  IEmployeeService _employeeService;
        private readonly IFileService _fileService;

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Employee View
        public IActionResult Employee()
        {
            return View();
        }

        #endregion

        #region Constructor
        public EmployeeController(IEmployeeValidator employeeValidator, IEmployeeService employeeService, ApplicationDbContext db, 
            IFileService fileService, ILogger<EmployeeController> logger, IEmployeeRepository emp, IWebHostEnvironment env)
        {
            _db = db;
            _employeeValidator = employeeValidator;
            _employeeService = employeeService;
            _fileService = fileService;
            _logger = logger;
            _emp = emp;
             _env = env;
            
        }
        #endregion

        #region Get Employees
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _emp.GetAllEmpAsync();

                var data = employees.Select(x => new
                {
                    rowId = x.Id,
                    email = x.Email,
                    country = x.Country?.CountryName ?? "",
                    state = x.State?.StateName ?? "",
                    city = x.City?.CityName ?? "",
                    panNo = x.PanNumber,
                    passportNo = x.PassportNumber,
                    gender = x.Gender == 1 ? "Male" : "Female",
                    isActive = x.IsActive == true ? "Yes" : "No",
                    profileImage = x.ProfileImage
                });

                return Json(new { data });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new { success = false, message = "Error while getting all employees." });
            }
          
        }
        #endregion

        #region Load all countries
        [HttpGet]
        public async Task<JsonResult> GetCountries()
        {
            try
            {
                var countries = await _db.Countries
               .Select(c => new { c.CountryId, c.CountryName })
               .ToListAsync();

                return Json(countries);
            }
            catch (Exception ex)
            {               
                _logger.LogError(ex.Message);
                return Json(new { success = false, message = "Error while load Countries." });
            }
           
        }
        #endregion

        #region Load states based on selected country
        [HttpGet]
        public async Task<JsonResult> GetStatesByCountry(int countryId)
        {
            try
            {
                var states = await _db.States
               .Where(s => s.CountryId == countryId)
               .Select(s => new { s.StateId, s.StateName })
               .ToListAsync();

                return Json(states);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new { success = false, message = "Error while load states." });
            }
           
        }
        #endregion

        #region Load cities based on selected state
        [HttpGet]
        public async Task<JsonResult> GetCitiesByState(int stateId)
        {
            try
            {
                var cities = await _db.Cities
                .Where(c => c.Stateid == stateId)
                .Select(c => new { c.CityId, c.CityName })
                .ToListAsync();

                return Json(cities);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new { success = false, message = "Error while load cities." });
            }
            
        }
        #endregion

        #region Load Employee data based on Id
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            try
            {
                if (id == 0)
                    return PartialView("Employee", new EmployeeMaster());

                var data = await _emp.GetbyIdAsync(id);

                if (data is null)
                    return NotFound();

                var model = new EmployeeMaster
                {
                    Id = data.Id,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Email,
                    PassportNumber = data.PassportNumber,
                    PanNumber = data.PanNumber,
                    MobileNumber = data.MobileNumber,
                    DateOfBirth = data.DateOfBirth,
                    DateOfJoinee = data.DateOfJoinee,
                    CountryID = data.CountryID,
                    StateID = data.StateID,
                    CityID = data.CityID,
                    ProfileImage = data.ProfileImage,
                    Gender = data.Gender,
                    IsActive = data.IsActive,
                    Country = data.Country,
                    State = data.State,
                    City = data.City

                };

                if (!string.IsNullOrEmpty(model.ProfileImage) && model.ProfileImage.Length > 0)
                {
                    var fileName = Path.GetFileName(model.ProfileImage);
                }

                ViewBag.SelectedCountryId = model.CountryID;
                ViewBag.SelectedStateId = model.StateID;
                ViewBag.SelectedCityId = model.CityID;

                return View("Employee", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new { success = false, message = "Error while load selected employee." });
            }
        }
        #endregion

        #region Add or Edit Employee
        /// <summary>
        /// Add or Edit Employee method using SOLID Principle.
        /// 
        /// Topics covered -
        /// 
        /// 1) Single Responsibility Principle: 
        /// - Validation is delegated to IEmployeeValidator:
        /// - Business logic is delegated to IEmployeeService.
        /// 
        /// 2) Open/Closed Principle :
        /// - Add/Update logic can change inside  IEmployeeService without modifying this
        ///   controller method.
        ///   
        /// 3) Liskov Substitution Principle :
        /// - Any class implementing IEmployeeService or IEmployeeValidator can replace the current
        ///   implementations without breaking behaviour
        ///   
        /// 4) Interface Segregation Principle :
        /// - Controller depends only on small, specific interfaces (IEmployeeService, IEmployeeValidator).
        /// 
        /// 5) Dependency Inversion Principle :
        /// - Controller depends on abstractions, not implemenations. All dependencies injected.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> AddEdit(EmployeeMaster obj)
        {
            try
            {
                //SRP : Validation handled by a seperate validator service. 
                var result = await _employeeValidator.ValidateEmpAsync(obj);

                if (!result.IsValid)
                    return Json(new { success = false, message = result.Message });

                //SRP + OCP : Controller does not perform business logic itself.
                //Service decides how adding/updating is implemented.
                if (obj.Id == 0)
                    await _employeeService.AddEmployeeAsync(obj);
                else
                {
                    if (Request.Form["IsDeleteImage"] == "true")
                        obj.IsImageDeleted = true;

                    await _employeeService.UpdateEmployeeAsync(obj);
                }
                    
                return Json(new { success = true, message = "Employee added." });
            }
            catch (Exception ex)
            {
                //SRP : Logging is handled by injected logger service.
                _logger.LogError(ex, "Error updating employee with Id {Id}", obj.Id);
                return Json(new { success = false, message = "Something went wrong." });
            }
        }
        #endregion

        #region Delete Employee
        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                _logger.LogInformation("Deleting employee with id {Id}", id);
                await _emp.DeleteAsync(id);

                return Json(new { success = true, message = "Employee deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error while deleting employee with id {Id}", id, ex.Message);
                return Json(new { success = false, message = "Error while deleting employee: " + ex.Message });
            }
        }
        #endregion
    }
}
