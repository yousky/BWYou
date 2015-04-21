using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using System.Web.Http.ModelBinding;

namespace BWYou.Web.MVC.Services
{
    public interface IEntityService<TEntity> : IDisposable
    {

        IEnumerable<TEntity> GetFilteredList(TEntity model);
        IEnumerable<TEntity> GetFilteredList(TEntity model, string sort);
        IPagedList<TEntity> GetFilteredList(TEntity model, string sort, int pageNumber, int pageSize);

        IQueryable<TEntity> Query();

        IEnumerable<TEntity> GetList();
        Task<IEnumerable<TEntity>> GetListAsync();
        IPagedList<TEntity> GetList(string sort, int pageNumber, int pageSize);

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
