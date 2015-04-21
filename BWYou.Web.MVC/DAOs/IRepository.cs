using BWYou.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.DAOs
{
    public interface IRepository<TEntity>
        where TEntity : BWModel
    {
        TEntity Find(params object[] keyValues);
        Task<TEntity> FindAsync(params object[] keyValues);
        IQueryable<TEntity> Query { get; }
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Update(TEntity entity, params string[] updateProperties);
        void UpdateExceptNullValue(TEntity entity);
        void UpdateExceptNullValue(TEntity entity, params string[] updateProperties);
        void Remove(TEntity entity);
        void Reload(TEntity entity);
        void Clone(TEntity source);

    }
}
