using BWYou.Web.MVC.Models;
using System;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.DAOs
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity, TId> GetRepository<TEntity, TId>()
            where TEntity : IdModel<TId>;
        int SaveChanges();
        Task SaveChangesAsync();
    }
}
