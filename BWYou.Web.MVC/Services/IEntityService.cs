using BWYou.Web.MVC.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace BWYou.Web.MVC.Services
{
    public interface IEntityService<TEntity, TId> : IDisposable
        where TEntity : IdModel<TId>
    {
        Task<IEnumerable<TEntity>> GetFilteredListAsync(TEntity model);
        Task<IEnumerable<TEntity>> GetFilteredListAsync(TEntity model, string sort);
        Task<IPagedList<TEntity>> GetFilteredListAsync(TEntity model, string sort, int pageNumber, int pageSize);

        Task<IEnumerable<TEntity>> GetFilteredListAsync(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetFilteredListAsync(Expression<Func<TEntity, bool>> filter, string sort);
        Task<IPagedList<TEntity>> GetFilteredListAsync(Expression<Func<TEntity, bool>> filter, string sort, int pageNumber, int pageSize);

        IQueryable<TEntity> Query();

        IQueryable<TEntity> GetFilteredQuery(TEntity model);
        IOrderedQueryable<TEntity> GetFilteredQuery(TEntity model, string sort);
        IQueryable<TEntity> GetFilteredQuery(Expression<Func<TEntity, bool>> filter);
        IOrderedQueryable<TEntity> GetFilteredQuery(Expression<Func<TEntity, bool>> filter, string sort);

        Task<IEnumerable<TEntity>> GetListAsync();
        Task<IPagedList<TEntity>> GetListAsync(string sort, int pageNumber, int pageSize);

        Task<IEnumerable<TEntity>> GetListAsync(string sort);

        Task<TEntity> GetAsync(TId id);

        Task<TEntity> ValidAndCreateAsync(TEntity entity, ModelStateDictionary ModelState);

        Task<TEntity> ValidAndUpdateAsync(TEntity entity, ModelStateDictionary ModelState);

        Task<TEntity> ValidAndDeleteAsync(TId id, ModelStateDictionary ModelState);

        Task<TEntity> ValidAndCloneAsync(TId id, ModelStateDictionary ModelState);



    }
}
