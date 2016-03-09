namespace Rambler.Cinema.EntFrameworkDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        Line1 = c.String(nullable: false, maxLength: 512),
                        Line2 = c.String(maxLength: 512),
                        CityId = c.Int(nullable: false),
                        ZipCode = c.String(nullable: false, maxLength: 10, unicode: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.City", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.ZipCode, name: "IX_Address_ZipCode");
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CityId)
                .Index(t => t.Name, unique: true, name: "UX_City_Name");
            
            CreateTable(
                "dbo.Cinema",
                c => new
                    {
                        CinemaId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        HallsNumber = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        SupervisorId = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CinemaId)
                .ForeignKey("dbo.Address", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.SupervisorId)
                .Index(t => t.Name, unique: true, name: "UX_Cinema_Name")
                .Index(t => t.AddressId)
                .Index(t => t.SupervisorId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        GivenName = c.String(maxLength: 40),
                        MiddleName = c.String(maxLength: 40),
                        SurName = c.String(nullable: false, maxLength: 40),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        Title = c.String(maxLength: 64),
                        DepartmentId = c.Int(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Department", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => new { t.SurName, t.GivenName, t.MiddleName }, name: "UX_Person_Fio")
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId)
                .Index(t => t.Name, unique: true, name: "UX_Department_Name");
            
            CreateTable(
                "dbo.Phone",
                c => new
                    {
                        PhoneId = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false, maxLength: 32, unicode: false),
                        PhoneType = c.Int(),
                        PersonId = c.Int(),
                        CinemaId = c.Int(),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PhoneId)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Cinema", t => t.CinemaId, cascadeDelete: true)
                .Index(t => t.Number, name: "UX_Phone_Number")
                .Index(t => t.PersonId)
                .Index(t => t.CinemaId);
            
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        FilmId = c.Int(nullable: false),
                        CinemaId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.FilmId, t.CinemaId })
                .ForeignKey("dbo.Cinema", t => t.CinemaId, cascadeDelete: true)
                .ForeignKey("dbo.Film", t => t.FilmId, cascadeDelete: true)
                .Index(t => t.FilmId)
                .Index(t => t.CinemaId);
            
            CreateTable(
                "dbo.Film",
                c => new
                    {
                        FilmId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        YearFilmed = c.Int(nullable: false),
                        ProducerId = c.Int(nullable: false),
                        DurationMinutes = c.Int(nullable: false),
                        GenreId = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FilmId)
                .ForeignKey("dbo.Genre", t => t.GenreId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.ProducerId, cascadeDelete: true)
                .Index(t => t.Name, name: "IX_Film_Name")
                .Index(t => t.YearFilmed, name: "IX_Film_YearFilmed")
                .Index(t => t.ProducerId)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.Genre",
                c => new
                    {
                        GenreId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GenreId)
                .Index(t => t.Name, unique: true, name: "UX_Genre_Name");
            
            CreateTable(
                "dbo.Cinema_ContactPerson",
                c => new
                    {
                        CinemaId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CinemaId, t.PersonId })
                .ForeignKey("dbo.Cinema", t => t.CinemaId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.CinemaId)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cinema", "SupervisorId", "dbo.Person");
            DropForeignKey("dbo.Phone", "CinemaId", "dbo.Cinema");
            DropForeignKey("dbo.Session", "FilmId", "dbo.Film");
            DropForeignKey("dbo.Film", "ProducerId", "dbo.Person");
            DropForeignKey("dbo.Film", "GenreId", "dbo.Genre");
            DropForeignKey("dbo.Session", "CinemaId", "dbo.Cinema");
            DropForeignKey("dbo.Cinema_ContactPerson", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Cinema_ContactPerson", "CinemaId", "dbo.Cinema");
            DropForeignKey("dbo.Phone", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Person", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.Cinema", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Address", "CityId", "dbo.City");
            DropIndex("dbo.Cinema_ContactPerson", new[] { "PersonId" });
            DropIndex("dbo.Cinema_ContactPerson", new[] { "CinemaId" });
            DropIndex("dbo.Genre", "UX_Genre_Name");
            DropIndex("dbo.Film", new[] { "GenreId" });
            DropIndex("dbo.Film", new[] { "ProducerId" });
            DropIndex("dbo.Film", "IX_Film_YearFilmed");
            DropIndex("dbo.Film", "IX_Film_Name");
            DropIndex("dbo.Session", new[] { "CinemaId" });
            DropIndex("dbo.Session", new[] { "FilmId" });
            DropIndex("dbo.Phone", new[] { "CinemaId" });
            DropIndex("dbo.Phone", new[] { "PersonId" });
            DropIndex("dbo.Phone", "UX_Phone_Number");
            DropIndex("dbo.Department", "UX_Department_Name");
            DropIndex("dbo.Person", new[] { "DepartmentId" });
            DropIndex("dbo.Person", "UX_Person_Fio");
            DropIndex("dbo.Cinema", new[] { "SupervisorId" });
            DropIndex("dbo.Cinema", new[] { "AddressId" });
            DropIndex("dbo.Cinema", "UX_Cinema_Name");
            DropIndex("dbo.City", "UX_City_Name");
            DropIndex("dbo.Address", "IX_Address_ZipCode");
            DropIndex("dbo.Address", new[] { "CityId" });
            DropTable("dbo.Cinema_ContactPerson");
            DropTable("dbo.Genre");
            DropTable("dbo.Film");
            DropTable("dbo.Session");
            DropTable("dbo.Phone");
            DropTable("dbo.Department");
            DropTable("dbo.Person");
            DropTable("dbo.Cinema");
            DropTable("dbo.City");
            DropTable("dbo.Address");
        }
    }
}
