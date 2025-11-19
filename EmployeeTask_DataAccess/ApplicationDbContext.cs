

using EmployeeTask.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTask.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }

        public DbSet<EmployeeMaster> EmployeeMasters { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<City> Cities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeMaster>()
                .HasOne(e => e.Country)
                .WithMany()
                .HasForeignKey(e => e.CountryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeMaster>()
                .HasOne(e => e.State)
                .WithMany()
                .HasForeignKey(e => e.StateID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeMaster>()
                .HasOne(e => e.City)
                .WithMany()
                .HasForeignKey(e => e.CityID)
                .OnDelete(DeleteBehavior.Restrict);


            //unique fields
            modelBuilder.Entity<EmployeeMaster>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<EmployeeMaster>()
                .HasIndex(e => e.MobileNumber)
                .IsUnique();

            modelBuilder.Entity<EmployeeMaster>()
                .HasIndex(e => e.PanNumber)
                .IsUnique();

            modelBuilder.Entity<EmployeeMaster>()
                .HasIndex(e => e.PassportNumber)
                .IsUnique();

          
            

        //seed data
        modelBuilder.Entity<Country>().HasData(
                new Country { CountryId = 1, CountryName = "India" },
                 new Country { CountryId = 2, CountryName = "USA" },
                  new Country { CountryId = 3, CountryName = "England" }
                );

            modelBuilder.Entity<State>().HasData(
                new State { StateId = 1, CountryId=1,  StateName = "Maharashtra" },
                new State { StateId = 2, CountryId = 1, StateName = "Goa" },
                 new State { StateId = 3, CountryId = 2, StateName = "California" },
                  new State { StateId = 4, CountryId = 2, StateName = "New York" },
                   new State { StateId = 5, CountryId = 3, StateName = "London" },
                    new State { StateId = 6, CountryId = 3, StateName = "Lords" }

                );


            modelBuilder.Entity<City>().HasData(
                new City { CityId = 1, Stateid = 1, CityName = "Mumbai" },
                new City { CityId = 2, Stateid = 1, CityName = "Thane" },
                new City { CityId = 3, Stateid = 1, CityName = "Ambernath" },
                new City { CityId = 4, Stateid = 2, CityName = "Panji" },
                new City { CityId = 5, Stateid = 3, CityName = "Los Angeles" },
                new City { CityId = 6, Stateid = 4, CityName = "Addison" },
                new City { CityId = 7, Stateid = 5, CityName = "Birmingham" },
                new City { CityId = 8, Stateid = 6, CityName = "Manchester" }
            );

        }

       
    }
}
