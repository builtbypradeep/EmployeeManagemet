namespace EmployeeTask.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public ICollection<State> states { get; set; }
    }
}
