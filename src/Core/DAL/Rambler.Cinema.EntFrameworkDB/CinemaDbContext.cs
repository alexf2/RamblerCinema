﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using Rambler.Cinema.DAL.Entities;

namespace Rambler.Cinema.EntFrameworkDB
{
    public class CinemaDbContext: EntityDbContextBase, ICinemaDbContext
    {
        static CinemaDbContext()
        {
            Database.SetInitializer<CinemaDbContext>(new CustomDbInitializer());
        }

        public CinemaDbContext()
            : this("Cinema")
        {
            RegisterRemovers();
        }

        public CinemaDbContext(string connectString)
            : base(connectString)
        {
            Database.Log = x => Trace.WriteLine(x);
            RegisterRemovers();
        }

        public CinemaDbContext(DbConnection dbConnection)
            : base(dbConnection, true)
        {
            RegisterRemovers();
        }

        public virtual DbSet<City> Cities { get; set; }
        
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<ContactPerson> ContactPersons { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }

        public virtual DbSet<DAL.Entities.Cinema> Cinemas { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>(); //in order to generate tables in singular name
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Address>()
                .Property(e => e.ZipCode)
                .IsUnicode(false); //to use varchar instead of nvarchar 

            modelBuilder.Entity<Phone>()
                .Property(e => e.Number)
                .IsUnicode(false);

            //configure TPH: two kind of person in one table
            modelBuilder.Entity<Person>()
                .Map<Person>(cfg => cfg.Requires("Type").HasValue(0)) //type discriminator value
                .Map<ContactPerson>(cfg => cfg.Requires("Type").HasValue(1));

            //adding multicolumn index
            modelBuilder.Entity<Person>()
                .Property(e => e.SurName)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("UX_Person_Fio", 1)));
            modelBuilder.Entity<Person>()
                .Property(e => e.GivenName)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("UX_Person_Fio", 2)));
            modelBuilder.Entity<Person>()
                .Property(e => e.MiddleName)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("UX_Person_Fio", 3) { IsUnique = false, IsClustered = false }));
            

            //configure M:N
            modelBuilder.Entity<DAL.Entities.Cinema>()
              .HasMany<ContactPerson>(s => s.Contacts)
              .WithMany(c => c.Cinemas)
              .Map(cs =>
              {
                  cs.MapLeftKey("CinemaId");
                  cs.MapRightKey("PersonId");
                  cs.ToTable("Cinema_ContactPerson");
              });

            modelBuilder.Entity<DAL.Entities.Cinema>()
              .HasMany<Phone>(s => s.Phones)
              .WithMany(c => c.Cinemas)
              .Map(cs =>
              {
                  cs.MapLeftKey("CinemaId");
                  cs.MapRightKey("PhoneId");
                  cs.ToTable("Cinema_Phone");
              });

            modelBuilder.Entity<DAL.Entities.ContactPerson>()
              .HasMany<Phone>(s => s.Phones)
              .WithMany(c => c.ContactPersons)
              .Map(cs =>
              {
                  cs.MapLeftKey("PersonId");
                  cs.MapRightKey("PhoneId");
                  cs.ToTable("Person_Phone");
              });

            //turn off cascading del. on supervisor person
            modelBuilder.Entity<DAL.Entities.Cinema>()
                .HasRequired(e => e.Supervisor)                
                .WithMany()
                .HasForeignKey(e => e.SupervisorId)
                .WillCascadeOnDelete(false);


            /*modelBuilder.Entity<ContactPerson>()
                .HasMany(e => e.Phones)
                .WithOptional(e=>e.ContactPerson)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DAL.Entities.Cinema>()
                .HasMany(e => e.Phones)
                .WithOptional(e => e.Cinema)
                .WillCascadeOnDelete(false);*/


            /*modelBuilder.Entity<Phone>()
                .HasRequired<ContactPerson>(s => s.ContactPerson)
                .WithOptional(c => c.)
                .WillCascadeOnDelete(true);*/




            base.OnModelCreating(modelBuilder);
        }

        void RegisterRemovers()
        {
            this.RegisterForDelete<Film>(f =>
            {
                f.CinemaSessions.RegisterDeleteOnRemove(this);                                
            });

            this.RegisterForDelete<DAL.Entities.Cinema>(f =>
            {
                f.FilmSessions.RegisterDeleteOnRemove(this);  
                f.Phones.RegisterDeleteOnRemove(this);
                //f.Contacts.RegisterDeleteOnRemove(this);
            });

            this.RegisterForDelete<ContactPerson>(f =>
            {
                f.Phones.RegisterDeleteOnRemove(this);
            });
        }
    }
}
