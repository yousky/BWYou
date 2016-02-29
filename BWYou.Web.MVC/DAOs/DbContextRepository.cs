using BWYou.Web.MVC.Attributes;
using BWYou.Web.MVC.Extensions;
using BWYou.Web.MVC.Models;
using log4net;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.DAOs
{
    /// <summary>
    /// IRepository 구현한 공통 구현체
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DbContextRepository<TEntity> : IRepository<TEntity>
        where TEntity : BWModel
    {
        /// <summary>
        /// 로그
        /// </summary>
        public ILog logger = LogManager.GetLogger(typeof(DbContextRepository<TEntity>));

        private DbContext _dbContext;
        private DbSet<TEntity> _dbSet;

        /// <summary>
        /// 생성자
        /// </summary>
        public DbContextRepository()
        {

        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="dbContext"></param>
        public DbContextRepository(DbContext dbContext)
        {
            this._dbContext = dbContext;
            this._dbSet = dbContext.Set<TEntity>();
        }
        /// <summary>
        /// PK 이용 Select
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public TEntity Find(params object[] keyValues)
        {
            return this._dbSet.Find(keyValues);
        }
        /// <summary>
        /// 비동기 PK 이용 Select
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public Task<TEntity> FindAsync(params object[] keyValues)
        {
            return this._dbSet.FindAsync(keyValues);
        }
        /// <summary>
        /// IQueryable 노출
        /// </summary>
        public IQueryable<TEntity> Query
        {
            get
            {
                return this._dbSet;
            }
        }
        /// <summary>
        /// 생성
        /// </summary>
        /// <param name="entity"></param>
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
            if (this._dbContext.Entry(entity).State != EntityState.Detached)
            {
                logger.Warn(string.Format("관리 되고 있는 Entity의 수동 업데이트 발생(Skip)  : type={0}, id={1}", entity.GetType().FullName, entity.Id));
                return;
            }

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
            if (this._dbContext.Entry(entity).State != EntityState.Detached)
            {
                logger.Warn(string.Format("관리 되고 있는 Entity의 수동 업데이트 발생(Skip)  : type={0}, id={1}", entity.GetType().FullName, entity.Id));
                return;
            }

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
        /// <param name="updateProperties"></param>
        public void UpdateExceptNullValue(TEntity entity, params string[] updateProperties)
        {
            if (this._dbContext.Entry(entity).State != EntityState.Detached)
            {
                logger.Warn(string.Format("관리 되고 있는 Entity 업데이트 무시 : type={0}, id={1}", entity.GetType().FullName, entity.Id));
                return;
            }

            this._dbContext.Entry(entity).State = EntityState.Unchanged;

            foreach (string name in updateProperties)
            {
                if (this._dbContext.Entry(entity).Property(name).CurrentValue != null)
                {
                    this._dbContext.Entry(entity).Property(name).IsModified = true;
                }
            }
        }
        /// <summary>
        /// 삭제. 연결 된 관계 자료도 삭제 하기 위하여 자동 활성화
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(TEntity entity)
        {
            entity.ActivateRelation4Cascade(new HashSet<object>()); //Lazy loading이나, 특이한 관계 구조 처리를 수동으로 하기 위하여 관계 활성화 필수
            this._dbSet.Remove(entity);
        }
        /// <summary>
        /// 다시 불러오기
        /// </summary>
        /// <param name="entity"></param>
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
        /// <summary>
        /// DBSet 노출
        /// </summary>
        public DbSet<TEntity> DBSet
        {
            get
            {
                return this._dbSet;
            }
        }
        /// <summary>
        /// DbContext 노출
        /// </summary>
        public DbContext DBContext
        {
            get
            {
                return this._dbContext;
            }
        }
    }
}
