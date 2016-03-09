using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Rambler.Cinema.DAL.Entities;

namespace Rambler.Cinema.EntFrameworkDB.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using GenFu;

    internal sealed class Configuration : DbMigrationsConfiguration<Rambler.Cinema.EntFrameworkDB.CinemaDbContext>
    {
        sealed class SequenceIterator
        {
            int _val;
            public SequenceIterator(int start = 0)
            {
                _val = start;
            }
            public int Next()
            {
                return _val++;
            }
        }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed (CinemaDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //System.Diagnostics.Debugger.Launch();

            context.Configuration.AutoDetectChangesEnabled = false;

            var rnd = new Random();

            A.Configure<City>()
                .Fill(c => c.CityId, 0)
                .Fill(c => c.Name).AsCity();

            var depNames = new string[]
            {
                "Accounting", "Cashbox", "Security", "Public relations", "Advertisement", "Human resources", "Electricity",
                "Direction", "Projection", "Services"
            };
            var depaIterator = new SequenceIterator();
            A.Configure<Department>()
                .Fill(c => c.DepartmentId, 0)
                .Fill(c => c.Name, (d) => depNames[ depaIterator.Next() ]);
            

            var cities = A.ListOf<City>(50).DistinctBy(c => c.Name).ToList();
            var deps = A.ListOf<Department>(depNames.Length).ToList();

            context.Cities.AddRange(cities);
            context.Departments.AddRange(deps);

            A.Configure<Phone>()
                .Fill(c => c.PhoneId, 0)
                .Fill(c => c.Number).AsPhoneNumber()
                .Fill(c => c.PhoneType).WithRandom(new PhoneType?[]{PhoneType.Work, PhoneType.Home, PhoneType.Cell, PhoneType.Main, PhoneType.Additional});            

            A.Configure<Person>()
                .Fill(c => c.PersonId, 0)
                .Fill(c => c.GivenName).AsFirstName()
                .Fill(c => c.SurName).AsLastName()
                .Fill(c => c.MiddleName).AsFirstName();

            A.Configure<ContactPerson>()
                .Fill(c => c.PersonId, 0)
                .Fill(c => c.GivenName).AsFirstName()
                .Fill(c => c.SurName).AsLastName()
                .Fill(c => c.MiddleName).AsFirstName()
                .Fill(c => c.Title)
                .WithRandom(new string[]
                {
                    "Accountant", "Cashir", "Guard", "PR manager", "Booster", "HR", "Electrician", "Director",
                    "Mechanicr",
                    "Usher", "Cleaning woman"
                })
                .Fill(c => c.Department, (c) => DepByTitle(c.Title, deps))
                .Fill(c => c.Phones, GetPhones(rnd.Next(1, 4)));

            var persons = A.ListOf<Person>(100).ToList();
            var cpersons = A.ListOf<ContactPerson>(100).ToList();            


            var genres = new Genre[]
            {
                new Genre() { Name = "Action"}, new Genre() { Name = "Thriller" }, new Genre() { Name = "Comedy" }, new Genre() { Name = "Documentary" },
                new Genre() { Name = "Fantastic" },
                new Genre() { Name = "Horror"}, new Genre() { Name = "Adventure"}, new Genre() { Name = "Drama"}, new Genre() { Name = "Science Fiction"},
                new Genre() { Name = "Western"}, new Genre() { Name = "Musical"}
            };

            context.Genres.AddRange(genres);                        

            A.Configure<Film>()
                .Fill(c => c.FilmId, 0)
                .Fill(c => c.Name).AsArticleTitle()
                .Fill(c => c.DurationMinutes)
                    .WithRandom(new int[] {20, 30, 45, 60, 60, 60, 60, 90, 90, 90, 120, 120, 120, 70, 70})
                .Fill(c => c.Genre).WithRandom(genres)
                .Fill(c => c.Producer).WithRandom(persons)
                .Fill(c => c.YearFilmed).WithinRange(1980, 2016);

            var films = A.ListOf<Film>(150).ToList();

            context.Films.AddRange(films);

            context.SaveChanges();            

            var filmNames = new []
            {
                "Cafe Oto",
                "Catford Constitutional Club",
                "Cine Lumiere",
                "The Cinema Museum",
                "Cineworld At The O2, Greenwich",
                "Cineworld Bexleyheath",
                "Cineworld Chelsea",
                "Cineworld Enfield",
                "Cineworld Feltham",
                "Cineworld Fulham Road",
                "Cineworld Hammersmith",
                "Cineworld Haymarket",
                "Cineworld Ilford",
                "Cineworld Staples Corner",
                "Cineworld Wandsworth",
                "Cineworld West India Quay",
                "Cineworld Wood Green",
                "Clapham Picturehouse",
                "Close-Up Cinema",
                "Conway Hall",
                "Crouch End Picturehouse",
                "Croydon Fairfield Halls",
                "Curzon Bloomsbury",
                "Curzon Chelsea",
                "Curzon Goldsmiths",
                "Curzon Mayfair",
                "Curzon Mondrian",
                "Curzon Richmond",
                "Curzon Soho",
                "Curzon Victoria",

                "Gate Picturehouse, Notting Hill",
                "Genesis Cinema",
                "Goethe-Institut",
                "Greenwich Picturehouse",

                "Ealing Town Hall",
                "East Dulwich Picturehouse",
                "Electric Cinema",
                "Electric Cinema Shoreditch",
                "Empire Bromley",
                "Empire Leicester Square",
                "Empire Sutton",
                "Everyman Baker Street",
                "Everyman Barnet",
                "Everyman Belsize Park",
                "Everyman Canary Wharf",
                "Everyman Esher",
                "Everyman Hampstead",
                "Everyman Maida Vale",
                "Everyman Muswell Hill",
                "Everyman Reigate",
                "Everyman Screen On The Green",
                "Everyman Walton On Thames",
                "Exhibit Bar & Restaurant"
            };

            A.Configure<Address>()
                .Fill(c => c.AddressId, 0)
                .Fill(c => c.Line1).AsAddress()
                .Fill(c => c.Line2).AsAddressLine2()
                .Fill(c => c.City).WithRandom(cities)
                .Fill(c => c.ZipCode, (c) => GetZip(c));
            
            var cinemaIt = new SequenceIterator();
            A.Configure<DAL.Entities.Cinema>()
                .Fill(c => c.CinemaId, 0)
                .Fill(c => c.Name, (c) => filmNames[cinemaIt.Next()])
                .Fill(c => c.HallsNumber).WithRandom(new[] {1, 1, 1, 1, 1, 2, 2, 2, 3, 3, 4, 4, 5, 6})
                .Fill(c => c.Address, (ñ) => A.New<Address>())
                .Fill(c => c.Phones, (c) => GetPhones(rnd.Next(1, 6)))
                .Fill(c => c.Supervisor).WithRandom(persons)
                .Fill(c => c.Contacts, (c) => PullItems(cpersons, rnd.Next(1, 8)));        

            var cinemas = A.ListOf<DAL.Entities.Cinema>(filmNames.Length).ToList();

            context.Cinemas.AddRange(cinemas);

            context.Configuration.AutoDetectChangesEnabled = true;
            context.SaveChanges();
        }
                
        static ICollection<Phone> GetPhones(int count) => A.ListOf<Phone>(count).ToArray();


        static ICollection<T> PullItems<T>(IList<T> pull, int cnt) where T:class
        {            
            var res = new List<T>();
            for (int i = Math.Min(cnt, pull.Count); i > 0; --i)
            {
                var latsIdx = pull.Count - 1;                
                
                res.Add( pull[latsIdx] );
                pull.RemoveAt(latsIdx);
            }
            return res;
        }

        static readonly Dictionary<string, string> _depsMap = new Dictionary<string, string>()
        {
            { "Accountant","Accounting"},
            { "Cashir", "Cashbox"},
            { "Guard", "Security"},
            { "PR manager", "Public relations"},
            { "Booster", "Advertisement"},
            { "HR", "Human resources"},
            { "Electrician", "Electricity"},
            { "Director", "Direction"},
            { "Mechanicr", "Projection"},
            { "Usher", "Services"},
            { "Cleaning woman", "Services"}
        };
        static Department DepByTitle(string title, IList<Department> deps) => deps.First(d => String.Equals(d.Name, _depsMap[title], StringComparison.OrdinalIgnoreCase));

        static readonly Random _rnd = new Random();
        static string GetZip(Address a) => _rnd.Next(100000, 200000).ToString();
    }    
}
