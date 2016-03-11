using System.Collections.Generic;
using Rambler.Cinema.DAL.Entities;

namespace Rambler.Cinema.EntFrameworkDB.Migrations
{
    using System;
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

        readonly Random _rnd = new Random();

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(CinemaDbContext context)
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

            var cities = PopulateCities();
            var deps = PopulateDepartments();
            context.Cities.AddRange(cities);
            context.Departments.AddRange(deps);

            ConfigurePhones();

            var people = PopulatePeople();
            var contactPeople = PopulateContactPeople(deps);

            var genres = PopulateGenres();
            context.Genres.AddRange(genres);


            var films = PopulateFilms(genres, people);
            context.Films.AddRange(films);

            StoreContextData(context);

            ConfigureAddress(cities);

            var cinemas = PopulateCinemas(people, contactPeople);
            context.Cinemas.AddRange(cinemas);

            StoreContextData(context);

            DateTime dt = DateTime.Now;
            for (int i = 0; i < 10; ++i, dt = dt.AddDays(1))
            { 
                foreach (var c in cinemas)                
                    AddSesions(c, dt, films, _rnd.Next(1, 15));

                    StoreContextData(context);
            }
            context.Configuration.AutoDetectChangesEnabled = true;
        }

        
        IList<City> PopulateCities()
        {
            A.Configure<City>()
                .Fill(c => c.CityId, 0)
                .Fill(c => c.Name).AsCity();

            return  A.ListOf<City>(50).DistinctBy(c => c.Name).ToList();
        }

        IList<Department> PopulateDepartments()
        {
            var depNames = new string[]
            {
                "Accounting", "Cashbox", "Security", "Public relations", "Advertisement", "Human resources", "Electricity",
                "Direction", "Projection", "Services"
            };
            var depaIterator = new SequenceIterator();
            A.Configure<Department>()
                .Fill(c => c.DepartmentId, 0)
                .Fill(c => c.Name, (d) => depNames[depaIterator.Next()]);

            return A.ListOf<Department>(depNames.Length).ToList();
        }

        void ConfigurePhones()
        {
            A.Configure<Phone>()
                .Fill(c => c.PhoneId, 0)
                .Fill(c => c.Number).AsPhoneNumber()
                .Fill(c => c.PhoneType).WithRandom(new PhoneType?[] { PhoneType.Work, PhoneType.Home, PhoneType.Cell, PhoneType.Main, PhoneType.Additional });            
        }

        IList<Person> PopulatePeople()
        {
            A.Configure<Person>()
                .Fill(c => c.PersonId, 0)
                .Fill(c => c.GivenName).AsFirstName()
                .Fill(c => c.SurName).AsLastName()
                .Fill(c => c.MiddleName).AsFirstName();

            return A.ListOf<Person>(100).ToList();
        }

        IList<ContactPerson> PopulateContactPeople(IList<Department> deps)
        {
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
                .Fill(c => c.Phones, GetPhones(_rnd.Next(1, 4)));

            return A.ListOf<ContactPerson>(100).ToList();
        }

        IList<Genre> PopulateGenres() => new Genre[]
        {
            new Genre() { Name = "Action"}, new Genre() { Name = "Thriller" }, new Genre() { Name = "Comedy" }, new Genre() { Name = "Documentary" },
            new Genre() { Name = "Fantastic" }, new Genre() { Name = "Horror"}, new Genre() { Name = "Adventure"},
            new Genre() { Name = "Drama"}, new Genre() { Name = "Science Fiction"}, new Genre() { Name = "Western"}, new Genre() { Name = "Musical"}
        };

        IList<Film> PopulateFilms (IList<Genre> genres, IList<Person> people)
        {
            A.Configure<Film>()
                .Fill(c => c.FilmId, 0)
                .Fill(c => c.Name).AsArticleTitle()
                .Fill(c => c.DurationMinutes)
                    .WithRandom(new int[] { 20, 30, 45, 60, 60, 60, 60, 90, 90, 90, 120, 120, 120, 70, 70 })
                .Fill(c => c.Genre).WithRandom(genres)
                .Fill(c => c.Producer).WithRandom(people)
                .Fill(c => c.YearFilmed).WithinRange(1980, 2016);

            return A.ListOf<Film>(150).ToList();
        }

        void ConfigureAddress(IList<City> cities)
        {
            A.Configure<Address>()
                .Fill(c => c.AddressId, 0)
                .Fill(c => c.Line1).AsAddress()
                .Fill(c => c.Line2).AsAddressLine2()
                .Fill(c => c.City).WithRandom(cities)
                .Fill(c => c.ZipCode, (c) => GetZip(c));
        }

        void StoreContextData (CinemaDbContext context)
        {
            context.Configuration.AutoDetectChangesEnabled = true;
            context.SaveChanges();
            context.Configuration.AutoDetectChangesEnabled = false;
        }

        IList<DAL.Entities.Cinema> PopulateCinemas(IList<Person> people, IList<ContactPerson> contactPeople)
        {
            var filmNames = new[]
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



            var cinemaIt = new SequenceIterator();
            A.Configure<DAL.Entities.Cinema>()
                .Fill(c => c.CinemaId, 0)
                .Fill(c => c.Name, (c) => filmNames[cinemaIt.Next()])
                .Fill(c => c.HallsNumber).WithRandom(new[] { 1, 1, 1, 1, 1, 2, 2, 2, 3, 3, 4, 4, 5, 6 })
                .Fill(c => c.Address, (ñ) => A.New<Address>())
                .Fill(c => c.Phones, (c) => GetPhones(_rnd.Next(1, 6)))
                .Fill(c => c.Supervisor).WithRandom(people)
                .Fill(c => c.Contacts, (c) => PullItems(contactPeople, _rnd.Next(1, 8)));

            return A.ListOf<DAL.Entities.Cinema>(filmNames.Length).ToList();
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

        static readonly Random _srnd = new Random();
        static string GetZip(Address a) => _srnd.Next(100000, 200000).ToString();

        const int DayStart = 9 * 60, DayEnd = 22 * 60, SessionAlignment = 30, MaxReallocate = 50;

        class FilmSession : IComparable, IComparable<FilmSession>
        {
            public Film Film;
            public int TimeLabel;
            public bool IsStart;

            public override int GetHashCode() => 31 ^ TimeLabel.GetHashCode() ^ IsStart.GetHashCode();

            public int CompareTo(object obj) => CompareTo((FilmSession)obj);

            public int CompareTo(FilmSession other)
            {
                if (other == null)
                    return 1;
                return TimeLabel + (IsStart ? -1 : 1) - other.TimeLabel - (other.IsStart ? -1 : 1);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                    return true;
                var o = obj as FilmSession;
                return TimeLabel == o?.TimeLabel && IsStart == o?.IsStart;
            }
        }

        void AddSesions(DAL.Entities.Cinema cinema, DateTime dt, IList<Film> films, int number)
        {
            var timeTable = new List<FilmSession>();
            int retry = 0;
            DateTime day = dt.Date;
            int pointsCount = (DayEnd - DayStart) / SessionAlignment;

            while (number > 0 && retry < MaxReallocate)
            {
                var f = films[_rnd.Next(0, films.Count)];
                int sessionStartMul = _rnd.Next(0, pointsCount + 1);
                int sessionStart = DayStart + SessionAlignment * sessionStartMul;
                int sessionEnd = sessionStart + f.DurationMinutes;


                var sta = new FilmSession() { Film = f, TimeLabel = sessionStart, IsStart = true };
                var end = new FilmSession() { Film = f, TimeLabel = sessionEnd, IsStart = false };

                if (timeTable.Any(tt => ReferenceEquals(tt.Film, f) && tt.IsStart && tt.TimeLabel == sessionStart))
                {
                    retry++;
                    continue;
                }

                int idx = timeTable.BinarySearch(sta);
                if (idx > -1)
                    timeTable.Insert(idx, sta);
                else
                    timeTable.Insert(~idx, sta);

                idx = timeTable.BinarySearch(end);
                if (idx > -1)
                    timeTable.Insert(idx, end);
                else
                    timeTable.Insert(~idx, end);

                var maxFilms = FindMaxInt(timeTable);
                if (maxFilms > cinema.HallsNumber)
                {
                    retry++;
                    timeTable.RemoveAll((s) => ReferenceEquals(s, sta) || ReferenceEquals(s, end));
                }
                else
                {
                    retry = 0;
                    number--;
                }
            }

            foreach (var fs in timeTable)
                if (fs.IsStart)
                    cinema.FilmSessions.Add(new Session() { Cinema = cinema, Film = fs.Film, StartTime = day.AddMinutes(fs.TimeLabel)});
        }

        static int FindMaxInt(IList<FilmSession> lst)
        {
            int max = 0;
            int current = 0;
            foreach (var fs in lst)
            {
                if (fs.IsStart)
                    current++;
                else
                    current--;

                if (current > max)
                    max = current;
            }
            return max;
        }
    }    
}
