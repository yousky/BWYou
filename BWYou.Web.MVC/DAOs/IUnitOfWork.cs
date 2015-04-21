using BWYou.Web.MVC.Models;
using System;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.DAOs
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BWModel;
        int SaveChanges();
        Task SaveChangesAsync();
    }
}
