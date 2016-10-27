using BWYou.Web.MVC.Attributes;
using BWYou.Web.MVC.DAOs;
using BWYou.Web.MVC.Etc;
using BWYou.Web.MVC.Extensions;
using BWYou.Web.MVC.Models;
using LinqKit;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace BWYou.Web.MVC.Services
{
    public class IdEntityService<TEntity, TId> : IEntityService<TEntity, TId>
        where TEntity : IdModel<TId>
    {
        protected IUnitOfWork _unitOfWork;
        protected IRepository<TEntity, TId> _repo;

        public IdEntityService(DbContext dbContext)
            : this(new DbContextUnitOfWork<DbContext>(dbContext))
        {

        }

        public IdEntityService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._repo = unitOfWork.GetRepository<TEntity, TId>();
        }

        public virtual Task<IEnumerable<TEntity>> GetFilteredListAsync(TEntity model)
        {
            var predicate = model.GetWhereClause();
            return GetFilteredListAsync(predicate);
        }
        public virtual Task<IEnumerable<TEntity>> GetFilteredListAsync(TEntity model, string sort)
        {
            var predicate = model.GetWhereClause();
            return GetFilteredListAsync(predicate, sort);
        }
        public virtual Task<IPagedList<TEntity>> GetFilteredListAsync(TEntity model, string sort, int pageNumber, int pageSize)
        {
            var predicate = model.GetWhereClause();
            return GetFilteredListAsync(predicate, sort, pageNumber, pageSize);
        }
        public virtual async Task<IEnumerable<TEntity>> GetFilteredListAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await this._repo.Query.AsExpandable().Where(filter).ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetFilteredListAsync(Expression<Func<TEntity, bool>> filter, string sort)
        {
            return await this._repo.Query.AsExpandable().Where(filter).SortBy(sort).ToListAsync();
        }
        public virtual async Task<IPagedList<TEntity>> GetFilteredListAsync(Expression<Func<TEntity, bool>> filter, string sort, int pageNumber, int pageSize)
        {
            return await Task<IPagedList<TEntity>>.Run(() =>
            {
                IOrderedQueryable<TEntity> orderedQueryable = this._repo.Query.AsExpandable().Where(filter).SortBy(sort);
                return orderedQueryable.ToPagedList(pageNumber, pageSize);
            });
        }

        public virtual IQueryable<TEntity> Query()
        {
            return this._repo.Query;
        }

        public virtual IQueryable<TEntity> GetFilteredQuery(TEntity model)
        {
            var predicate = model.GetWhereClause();
            return GetFilteredQuery(predicate);
        }
        public virtual IOrderedQueryable<TEntity> GetFilteredQuery(TEntity model, string sort)
        {
            var predicate = model.GetWhereClause();
            return GetFilteredQuery(predicate, sort);
        }

        public virtual IQueryable<TEntity> GetFilteredQuery(Expression<Func<TEntity, bool>> filter)
        {
            return this._repo.Query.AsExpandable().Where(filter);
        }
        public virtual IOrderedQueryable<TEntity> GetFilteredQuery(Expression<Func<TEntity, bool>> filter, string sort)
        {
            return this._repo.Query.AsExpandable().Where(filter).SortBy(sort);
        }

        public virtual async Task<IEnumerable<TEntity>> GetListAsync()
        {
            return await this._repo.Query.ToListAsync();
        }
        public virtual async Task<IPagedList<TEntity>> GetListAsync(string sort, int pageNumber, int pageSize)
        {
            return await Task<IPagedList<TEntity>>.Run(() =>
            {
                IOrderedQueryable<TEntity> orderedQueryable = this._repo.Query.SortBy(sort);
                return orderedQueryable.ToPagedList(pageNumber, pageSize);
            });
        }
        public virtual async Task<IEnumerable<TEntity>> GetListAsync(string sort)
        {
            return await this._repo.Query.AsExpandable().SortBy(sort).ToListAsync();
        }

        public virtual async Task<TEntity> GetAsync(TId id)
        {
            TEntity model = await this._repo.FindAsync(id);
            return model;
        }
        public virtual async Task<TEntity> ValidAndCreateAsync(TEntity model, ModelStateDictionary ModelState)
        {
            return await CreateAsync(model);
        }
        protected virtual async Task<TEntity> CreateAsync(TEntity model)
        {
            this._repo.Create(model);
            await this._unitOfWork.SaveChangesAsync();
            return model;
        }
        public virtual async Task<TEntity> ValidAndUpdateAsync(TEntity model, ModelStateDictionary ModelState)
        {
            return await UpdateAsync(model);
        }
        protected virtual async Task<TEntity> UpdateAsync(TEntity model)
        {
            this._repo.Update(model, GetUpdatablePropertiesNameArray(model));
            await this._unitOfWork.SaveChangesAsync();
            this._repo.Reload(model);   //model 중에 updatable만 업뎃 되기 때문에 새로 로드해야 DB값과 일치 됨
            return model;
        }
        public virtual async Task<TEntity> ValidAndDeleteAsync(TId id, ModelStateDictionary ModelState)
        {
            TEntity model = await this._repo.FindAsync(id);
            if (model == null)
            {
                ModelState.AddModelError("Id", "Id Not Exist");
                return null;
            }
            return await DeleteAsync(model);
        }
        protected virtual async Task<TEntity> DeleteAsync(TEntity model)
        {
            this._repo.Remove(model);
            await this._unitOfWork.SaveChangesAsync();
            return model;
        }
        public virtual async Task<TEntity> ValidAndCloneAsync(TId id, ModelStateDictionary ModelState)
        {
            TEntity model = await this._repo.FindAsync(id);
            if (model == null)
            {
                ModelState.AddModelError("Id", "Id Not Exist");
                return null;
            }
            return await CloneAsync(model);
        }
        protected virtual async Task<TEntity> CloneAsync(TEntity model)
        {
            this._repo.Clone(model);
            await this._unitOfWork.SaveChangesAsync();
            return model;
        }

        /// <summary>
        /// 살아 있는지 확인 하고 DB 연결 유지 하기 위하여 DB 쿼리 한번 시도
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TEntity> BeatAsync()
        {
            return await this._repo.Query.FirstOrDefaultAsync();
        }
        /// <summary>
        /// 업데이트 가능한 프로퍼티 이름들의 배열 획득
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string[] GetUpdatablePropertiesNameArray(TEntity model)
        {
            var props = model.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(UpdatableAttribute), true).Length != 0);
            List<string> updateProperties = new List<string>();
            foreach (var item in props)
            {
                updateProperties.Add(item.Name);
            }
            if (updateProperties.Count <= 0)
            {
                throw new Exception("Not Exist Updatable Properties");
            }
            return updateProperties.ToArray();
        }

        public void Dispose()
        {
            this._unitOfWork.Dispose();
        }
    }
}
