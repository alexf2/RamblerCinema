using System.Data.Entity;

namespace Rambler.Cinema.DAL.Entities
{
    public interface ICinemaDbContext
    {
        DbSet<City> Cities { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<Person> Persons { get; set; }
        DbSet<ContactPerson> ContactPersons { get; set; }
        DbSet<Phone> Phones { get; set; }
        DbSet<Genre> Genres { get; set; }

        DbSet<DAL.Entities.Cinema> Cinemas { get; set; }
        DbSet<Session> Sessions { get; set; }
        DbSet<Film> Films { get; set; }        
    }
}
