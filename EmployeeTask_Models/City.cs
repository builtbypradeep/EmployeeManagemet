using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTask.Models
{
    public class City
    {
        public int CityId { get; set; }

        [ForeignKey("State")]
        public int Stateid {  get; set; }
        public string CityName { get; set; }
        public State state { get; set; }
    }
}
