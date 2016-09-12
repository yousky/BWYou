using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace BWYou.Web.MVC.Services
{
    public interface IEntityService<TEntity> : IDisposable
    {

        IEnumerable<TEntity> GetFilteredList(TEntity model);
        Task<IEnumerable<TEntity>> GetFilteredListAsync(TEntity model);
        IEnumerable<TEntity> GetFilteredList(TEntity model, string sort);
        Task<IEnumerable<TEntity>> GetFilteredListAsync(TEntity model, string sort);
        IPagedList<TEntity> GetFilteredList(TEntity model, string sort, int pageNumber, int pageSize);
        Task<IPagedList<TEntity>> GetFilteredListAsync(TEntity model, string sort, int pageNumber, int pageSize);

        IEnumerable<TEntity> GetFilteredList(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetFilteredListAsync(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetFilteredList(Expression<Func<TEntity, bool>> filter, string sort);
        Task<IEnumerable<TEntity>> GetFilteredListAsync(Expression<Func<TEntity, bool>> filter, string sort);
        IPagedList<TEntity> GetFilteredList(Expression<Func<TEntity, bool>> filter, string sort, int pageNumber, int pageSize);
        Task<IPagedList<TEntity>> GetFilteredListAsync(Expression<Func<TEntity, bool>> filter, string sort, int pageNumber, int pageSize);

        IQueryable<TEntity> Query();

        IQueryable<TEntity> GetFilteredQuery(TEntity model);
        IOrderedQueryable<TEntity> GetFilteredQuery(TEntity model, string sort);
        IQueryable<TEntity> GetFilteredQuery(Expression<Func<TEntity, bool>> filter);
        IOrderedQueryable<TEntity> GetFilteredQuery(Expression<Func<TEntity, bool>> filter, string sort);

        IEnumerable<TEntity> GetList();
        Task<IEnumerable<TEntity>> GetListAsync();
        IPagedList<TEntity> GetList(string sort, int pageNumber, int pageSize);
        Task<IPagedList<TEntity>> GetListAsync(string sort, int pageNumber, int pageSize);

        IEnumerable<TEntity> GetList(string sort);
        Task<IEnumerable<TEntity>> GetListAsync(string sort);

        TEntity Get(int id);
        Task<TEntity> GetAsync(int id);

        TEntity ValidAndCreate(TEntity entity, ModelStateDictionary ModelState);
        Task<TEntity> ValidAndCreateAsync(TEntity entity, ModelStateDictionary ModelState);

        TEntity ValidAndUpdate(TEntity entity, ModelStateDictionary ModelState);
        Task<TEntity> ValidAndUpdateAsync(TEntity entity, ModelStateDictionary ModelState);

        TEntity ValidAndDelete(int id, ModelStateDictionary ModelState);
        Task<TEntity> ValidAndDeleteAsync(int id, ModelStateDictionary ModelState);

        TEntity ValidAndClone(int id, ModelStateDictionary ModelState);
        Task<TEntity> ValidAndCloneAsync(int id, ModelStateDictionary ModelState);



    }
}
