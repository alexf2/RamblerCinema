using System.Data.Entity;
using Rambler.Cinema.EntFrameworkDB.Migrations;

namespace Rambler.Cinema.EntFrameworkDB
{
    sealed class CustomDbInitializer: MigrateDatabaseToLatestVersion<CinemaDbContext, Configuration>
    {        
        public CustomDbInitializer (): base()
        {
        }

        public CustomDbInitializer(bool useSuppliedContext): base(useSuppliedContext)
        {
        }

        public CustomDbInitializer(bool useSuppliedContext, Configuration configuration): base(useSuppliedContext, configuration)
        {
        }

        public CustomDbInitializer(string connectionStringName): base(connectionStringName)
        {            
        }

        public override void InitializeDatabase (CinemaDbContext context)
        {
            base.InitializeDatabase(context);
            context.Database.ExecuteSqlCommand("CREATE NONCLUSTERED INDEX IX_Person_Type on Person (Type)");
        }
    }
}
