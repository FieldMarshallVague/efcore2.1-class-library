using MyApp.Interfaces.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MyApp
{
    public class EfRepository<TContext> : EfReadOnlyRepository<TContext>, IRepository
        where TContext : DbContext
    {
        public EfRepository(TContext context) : base(context)
        {
        }

        public virtual void Create<TEntity>(TEntity entity, string createdBy = null)
            where TEntity : class, IEntity
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedBy = createdBy;
            context.Set<TEntity>().Add(entity);
        }

        public virtual void Update<TEntity>(TEntity entity, string modifiedBy = null)
            where TEntity : class, IEntity
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.ModifiedBy = modifiedBy;
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete<TEntity>(object id)
            where TEntity : class, IEntity
        {
            TEntity entity = context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            var dbSet = context.Set<TEntity>();
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                ThrowDetailedUpdateException(e);
            }
        }

        public virtual Task SaveAsync()
        {
            try
            {
                return context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                ThrowDetailedUpdateException(e);
            }

            return Task.FromResult(0);
        }

        protected virtual void ThrowDetailedUpdateException(DbUpdateException e)
        {
            // todo: loop through data and create meaningful error list to pass to logging
            var errorMessages = e.Data.ToString();

            var fullErrorMessage = string.Join("; ", errorMessages);
            var exceptionMessage = string.Concat(e.Message, " The errors aren't working yet, this is just a classname... TODO!!!: ", fullErrorMessage);
            throw new DbUpdateException(exceptionMessage, e.InnerException);
        }
    }
}
