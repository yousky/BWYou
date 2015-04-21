using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Data.Entity;
using BWYou.Web.MVC.Models;
using BWYou.Web.MVC.Extensions;
using BWYou.Web.MVC.Attributes;

namespace BWYou.Web.MVC.DAOs
{
    public class DbContextRepository<TEntity> : IRepository<TEntity>
        where TEntity : BWModel
    {
        public ILog logger = LogManager.GetLogger(typeof(DbContextRepository<TEntity>));

        private DbContext _dbContext;
        private DbSet<TEntity> _dbSet;

        public DbContextRepository()
        {

        }

        public DbContextRepository(DbContext dbContext)
        {
            this._dbContext = dbContext;
            this._dbSet = dbContext.Set<TEntity>();
        }

        public TEntity Find(params object[] keyValues)
        {
            return this._dbSet.Find(keyValues);
        }
        public Task<TEntity> FindAsync(params object[] keyValues)
        {
            return this._dbSet.FindAsync(keyValues);
        }

        public IQueryable<TEntity> Query
        {
            get
            {
                return this._dbSet;
            }
        }

        public void Create(TEntity entity)
        {
            this._dbSet.Add(entity);
        }

        /// <summary>
        /// PK 제외 모든 칼럼 업데이트
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            this._dbContext.Entry(entity).State = EntityState.Modified;
        }
        /// <summary>
        /// 관리 되지 않고 있는 TEntity의 특정 칼럼만 업데이트.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updateProperties">원하는 칼럼 배열</param>
        public void Update(TEntity entity, params string[] updateProperties)
        {
            this._dbContext.Entry(entity).State = EntityState.Unchanged;

            foreach (string name in updateProperties)
            {
                this._dbContext.Entry(entity).Property(name).IsModified = true;
            }
        }

        /// <summary>
        /// 관리 되지 않고 있는 TEntity의 값이 있는 칼럼들 모두 업데이트.
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateExceptNullValue(TEntity entity)
        {
            this._dbContext.Entry(entity).State = EntityState.Modified;

            foreach (string name in this._dbContext.Entry(entity).CurrentValues.PropertyNames)
            {
                if (this._dbContext.Entry(entity).Property(name).CurrentValue == null)
                {
                    this._dbContext.Entry(entity).Property(name).IsModified = false;
                }
            }
        }

        /// <summary>
        /// 관리 되지 않고 있는 TEntity의 특정 칼럼중에서 값이 있는 칼럼들 모두 업데이트.
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateExceptNullValue(TEntity entity, params string[] updateProperties)
        {
            this._dbContext.Entry(entity).State = EntityState.Unchanged;

            foreach (string name in updateProperties)
            {
                if (this._dbContext.Entry(entity).Property(name).CurrentValue != null)
                {
                    this._dbContext.Entry(entity).Property(name).IsModified = true;
                }
            }
        }

        public void Remove(TEntity entity)
        {
            this._dbSet.Remove(entity);
        }

        public void Reload(TEntity entity)
        {
            this._dbContext.Entry(entity).Reload();
        }
        /// <summary>
        /// Entity를 복사하여 DB에 생성한다.
        /// </summary>
        /// <param name="source">복사 할 Entity</param>
        public void Clone(TEntity source)
        {
            logger.Info(string.Format("Clone Entity : type={0}, id={1}", source.GetType().FullName, source.Id));
            var clone = source.Clone(new Dictionary<object, object>(), false, true, CascadeRelationAttribute.CascadeDirection.Down, true);
            this._dbContext.Entry(source).State = EntityState.Detached;
            this._dbSet.Add(clone);
        }
    }
}
