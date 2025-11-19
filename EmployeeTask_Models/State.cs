//using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTask.Models
{
    public class State
    {
        public int StateId { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public string StateName { get; set; }
        public Country country { get; set; }
        public ICollection<City> cities { get; set; }
    }
}
