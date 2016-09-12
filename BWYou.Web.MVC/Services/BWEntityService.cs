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
    public class BWEntityService<TEntity> : IEntityService<TEntity>
        where TEntity : BWModel
    {
        protected IUnitOfWork _unitOfWork;
        protected IRepository<TEntity> _repo;

        public BWEntityService(DbContext dbContext)
            : this(new DbContextUnitOfWork<DbContext>(dbContext))
        {

        }

        public BWEntityService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._repo = unitOfWork.GetRepository<TEntity>();
        }


        public virtual IEnumerable<TEntity> GetFilteredList(TEntity model)
        {
            var predicate = GetWhereClause(model);
            return GetFilteredList(predicate);
        }
        public virtual Task<IEnumerable<TEntity>> GetFilteredListAsync(TEntity model)
        {
            var predicate = GetWhereClause(model);
            return GetFilteredListAsync(predicate);
        }
        public virtual IEnumerable<TEntity> GetFilteredList(TEntity model, string sort)
        {
            var predicate = GetWhereClause(model);
            return GetFilteredList(predicate, sort);
        }
        public virtual Task<IEnumerable<TEntity>> GetFilteredListAsync(TEntity model, string sort)
        {
            var predicate = GetWhereClause(model);
            return GetFilteredListAsync(predicate, sort);
        }
        public virtual IPagedList<TEntity> GetFilteredList(TEntity model, string sort, int pageNumber, int pageSize)
        {
            var predicate = GetWhereClause(model);
            return GetFilteredList(predicate, sort, pageNumber, pageSize);
        }
        public virtual Task<IPagedList<TEntity>> GetFilteredListAsync(TEntity model, string sort, int pageNumber, int pageSize)
        {
            var predicate = GetWhereClause(model);
            return GetFilteredListAsync(predicate, sort, pageNumber, pageSize);
        }

        public virtual IEnumerable<TEntity> GetFilteredList(Expression<Func<TEntity, bool>> filter)
        {
            return this._repo.Query.AsExpandable().Where(filter).ToList();
        }
        public virtual async Task<IEnumerable<TEntity>> GetFilteredListAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await this._repo.Query.AsExpandable().Where(filter).ToListAsync();
        }
        public virtual IEnumerable<TEntity> GetFilteredList(Expression<Func<TEntity, bool>> filter, string sort)
        {
            return this._repo.Query.AsExpandable().Where(filter).SortBy(sort).ToList();
        }
        public virtual async Task<IEnumerable<TEntity>> GetFilteredListAsync(Expression<Func<TEntity, bool>> filter, string sort)
        {
            return await this._repo.Query.AsExpandable().Where(filter).SortBy(sort).ToListAsync();
        }
        public virtual IPagedList<TEntity> GetFilteredList(Expression<Func<TEntity, bool>> filter, string sort, int pageNumber, int pageSize)
        {
            IOrderedQueryable<TEntity> orderedQueryable = this._repo.Query.AsExpandable().Where(filter).SortBy(sort);
            return orderedQueryable.ToPagedList(pageNumber, pageSize);
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
            var predicate = GetWhereClause(model);
            return GetFilteredQuery(predicate);
        }
        public virtual IOrderedQueryable<TEntity> GetFilteredQuery(TEntity model, string sort)
        {
            var predicate = GetWhereClause(model);
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


        public virtual IEnumerable<TEntity> GetList()
        {
            return this._repo.Query.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetListAsync()
        {
            return await this._repo.Query.ToListAsync();
        }
        public virtual IPagedList<TEntity> GetList(string sort, int pageNumber, int pageSize)
        {
            IOrderedQueryable<TEntity> orderedQueryable = this._repo.Query.SortBy(sort);
            return orderedQueryable.ToPagedList(pageNumber, pageSize);
        }
        public virtual async Task<IPagedList<TEntity>> GetListAsync(string sort, int pageNumber, int pageSize)
        {
            return await Task<IPagedList<TEntity>>.Run(() =>
            {
                IOrderedQueryable<TEntity> orderedQueryable = this._repo.Query.SortBy(sort);
                return orderedQueryable.ToPagedList(pageNumber, pageSize);
            });
        }

        public virtual IEnumerable<TEntity> GetList(string sort)
        {
            return this._repo.Query.AsExpandable().SortBy(sort).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetListAsync(string sort)
        {
            return await this._repo.Query.AsExpandable().SortBy(sort).ToListAsync();
        }

        public virtual TEntity Get(int id)
        {
            TEntity model = this._repo.Find(id);
            return model;
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            TEntity model = await this._repo.FindAsync(id);
            return model;
        }

        public virtual TEntity ValidAndCreate(TEntity model, ModelStateDictionary ModelState)
        {
            return Create(model);
        }

        public virtual async Task<TEntity> ValidAndCreateAsync(TEntity model, ModelStateDictionary ModelState)
        {
            return await CreateAsync(model);
        }

        protected virtual TEntity Create(TEntity model)
        {
            this._repo.Create(model);
            this._unitOfWork.SaveChanges();
            return model;
        }
        protected virtual async Task<TEntity> CreateAsync(TEntity model)
        {
            this._repo.Create(model);
            await this._unitOfWork.SaveChangesAsync();
            return model;
        }

        public virtual TEntity ValidAndUpdate(TEntity model, ModelStateDictionary ModelState)
        {
            if (false == this._repo.Query.Any(e => e.Id == model.Id))
            {
                ModelState.AddModelError("Id", "Id Not Exist");
                return null;
            }
            return Update(model);
        }
        public virtual async Task<TEntity> ValidAndUpdateAsync(TEntity model, ModelStateDictionary ModelState)
        {
            if (false == await this._repo.Query.AnyAsync(e => e.Id == model.Id))
            {
                ModelState.AddModelError("Id", "Id Not Exist");
                return null;
            }
            return await UpdateAsync(model);
        }

        protected virtual TEntity Update(TEntity model)
        {
            this._repo.Update(model, GetUpdatablePropertiesNameArray(model));
            this._unitOfWork.SaveChanges();
            this._repo.Reload(model);   //model 중에 updatable만 업뎃 되기 때문에 새로 로드해야 DB값과 일치 됨
            return model;
        }
        protected virtual async Task<TEntity> UpdateAsync(TEntity model)
        {
            this._repo.Update(model, GetUpdatablePropertiesNameArray(model));
            await this._unitOfWork.SaveChangesAsync();
            this._repo.Reload(model);
            return model;
        }

        public virtual TEntity ValidAndDelete(int id, ModelStateDictionary ModelState)
        {
            TEntity model = this._repo.Find(id);
            if (model == null)
            {
                ModelState.AddModelError("Id", "Id Not Exist");
                return null;
            }
            return Delete(model);
        }
        public virtual async Task<TEntity> ValidAndDeleteAsync(int id, ModelStateDictionary ModelState)
        {
            TEntity model = await this._repo.FindAsync(id);
            if (model == null)
            {
                ModelState.AddModelError("Id", "Id Not Exist");
                return null;
            }
            return await DeleteAsync(model);
        }

        protected virtual TEntity Delete(TEntity model)
        {
            this._repo.Remove(model);
            this._unitOfWork.SaveChanges();
            return model;
        }

        protected virtual async Task<TEntity> DeleteAsync(TEntity model)
        {
            this._repo.Remove(model);
            await this._unitOfWork.SaveChangesAsync();
            return model;
        }

        public virtual TEntity ValidAndClone(int id, ModelStateDictionary ModelState)
        {
            TEntity model = this._repo.Find(id);
            if (model == null)
            {
                ModelState.AddModelError("Id", "Id Not Exist");
                return null;
            }
            return Clone(model);
        }
        public virtual async Task<TEntity> ValidAndCloneAsync(int id, ModelStateDictionary ModelState)
        {
            TEntity model = await this._repo.FindAsync(id);
            if (model == null)
            {
                ModelState.AddModelError("Id", "Id Not Exist");
                return null;
            }
            return await CloneAsync(model);
        }

        protected virtual TEntity Clone(TEntity model)
        {
            this._repo.Clone(model);
            this._unitOfWork.SaveChanges();
            return model;
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
        public virtual Task<bool> BeatAsync()
        {
            return this._repo.Query.AnyAsync(e => e.Id == -1);
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

        /// <summary>
        /// 동적으로 Where 조건 만들기. model에서 FilterableAttribute이면서 값이 null이 아닌 것을 가지고.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> GetWhereClause(TEntity model)
        {
            List<ExpressionFilter> filter = new List<ExpressionFilter>();
            var props = model.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(FilterableAttribute), true).Length != 0);
            foreach (var prop in props)
            {
                var value = prop.GetValue(model);
                if (value != null)
                {
                    //Type t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    //Convert.ChangeType(value, prop.PropertyType);
                    filter.Add(new ExpressionFilter() { PropertyName = prop.Name, Operation = Op.Equals, Value = value });
                }
            }
            if (filter.Count <= 0)
            {
                throw new Exception("Not Exist Filter");
            }
            return ExpressionBuilder.GetExpression<TEntity>(filter);
            //var deleg = ExpressionBuilder.GetExpression<TEntity>(filter).Compile();   //Compile 하면 DB쪽으로 검색 안 하는 듯 함.. 뭐여.. ~_~;
            //return deleg;
        }

        private Expression<Func<TEntity, bool>> GetWhereClause2(TEntity model)
        {
            var predicate = ExpressionExtensions.BuildPredicate<TEntity>(model);
            //var predicate = ExpressionExtensions.BuildPredicate<TEntity, TEntity>(model);
            return predicate;
        }

        public void Dispose()
        {
            this._unitOfWork.Dispose();
        }
    }
}
