using BWYou.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.DAOs
{
    public class DbContextUnitOfWork<TDbContext> : IUnitOfWork
        where TDbContext : DbContext
    {
        private TDbContext _dbContext;
        private Dictionary<Type, object> _repositories;

        public DbContextUnitOfWork(TDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IRepository<TEntity, TId> GetRepository<TEntity, TId>()
            where TEntity : IdModel<TId>
        {
            if (this._repositories == null)
            {
                this._repositories = new Dictionary<Type, object>();
            }
            object repository;
            if (this._repositories.TryGetValue(typeof(TEntity), out repository) == false)
            {
                repository = new DbContextRepository<TEntity, TId>(this._dbContext);
                this._repositories[typeof(TEntity)] = repository;
            }
            return (IRepository<TEntity, TId>)repository;
        }

        public int SaveChanges()
        {
            return this._dbContext.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return this._dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }

    }
}
