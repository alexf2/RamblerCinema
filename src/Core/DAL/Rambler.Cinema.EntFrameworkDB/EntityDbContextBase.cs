using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rambler.Cinema.DAL.Entities;

namespace Rambler.Cinema.EntFrameworkDB
{
    public class EntityDbContextBase: DbContext
    {
        protected EntityDbContextBase() : base()
        {
        }

        protected EntityDbContextBase(DbCompiledModel model): base(model)
        {            
        }

        public EntityDbContextBase(string nameOrConnectionString): base(nameOrConnectionString)
        {            
        }

        public EntityDbContextBase(string nameOrConnectionString, DbCompiledModel model): base(nameOrConnectionString, model)
        {            
        }

        public EntityDbContextBase(DbConnection existingConnection, bool contextOwnsConnection): base(existingConnection, contextOwnsConnection)
        {            
        }

        public EntityDbContextBase(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection): base(existingConnection, model, contextOwnsConnection)
        {            
        }

        public EntityDbContextBase(ObjectContext objectContext, bool dbContextOwnsObjectContext): base(objectContext, dbContextOwnsObjectContext)
        {            
        }

        public override int SaveChanges()
        {
            try
            {
                AssignDates();
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw ExtractErrorsUp(ex);
            }
        }

        public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                AssignDates();
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException ex)
            {
                throw ExtractErrorsUp(ex);
            }
        }

        DbEntityValidationException ExtractErrorsUp(DbEntityValidationException ex)
        {
            var sb = new StringBuilder();

            foreach (var failure in ex.EntityValidationErrors)
            {
                sb.AppendFormat("Failed validation on entity [{0}]\n", failure.Entry.Entity.GetType());
                foreach (var error in failure.ValidationErrors)                
                    sb.AppendFormat("- [{0}] : {1}\r\n", error.PropertyName, error.ErrorMessage);
                                    
            }
            return new DbEntityValidationException("Entity Validation Failed - errors follow:\n" + sb, ex);
        }

        void AssignDates()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var baseEnt = (BaseEntity) entity.Entity;
                if (entity.State == EntityState.Added)
                    baseEnt.DateCreated = DateTime.UtcNow;

                baseEnt.DateUpdated = DateTime.UtcNow;
            }
        }
    }
}
