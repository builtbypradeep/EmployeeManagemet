
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTask.Models
{
    public class EmployeeMaster
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(8)]
        public string EmployeeCode {  get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [ForeignKey("Country")]
        public int CountryID { get; set; } 

        [Required(ErrorMessage = "State is required.")]
        [ForeignKey("State")]
        public int StateID { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [ForeignKey("City")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [MaxLength(15, ErrorMessage = "Mobile number cannot exceed 15 characters.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "PAN number is required.")]
        [MaxLength(12, ErrorMessage = "PAN number cannot exceed 12 characters.")]
        public string PanNumber { get; set; }

        [Required(ErrorMessage = "Passport number is required.")]
        [MaxLength(20, ErrorMessage = "Passport number cannot exceed 20 characters.")]
        public string PassportNumber { get; set; }

        //[Required(ErrorMessage = "Profile picture is required.")]
        [MaxLength(100, ErrorMessage = "Profile image path cannot exceed 100 characters.")]
        public string? ProfileImage { get; set; }


        [Required(ErrorMessage = "Please select gender.")]
        [Range(1, 2, ErrorMessage = "Please select gender.")]
        public byte? Gender { get; set; }

        [Required(ErrorMessage = "IsActive status is required.")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Date of joinee is required.")]
        public DateTime DateOfJoinee { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required(ErrorMessage = "IsDeleted status is required.")]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedDate { get; set; }

        public Country Country { get; set; }
        public State State { get; set; }
        public City City { get; set; }

        [NotMapped]
        //[Required(ErrorMessage = "Profile picture is required.")]
        public IFormFile? ImagePath { get; set; }

        [NotMapped]
        public bool IsImageDeleted { get; set; } = false;
    }
}
