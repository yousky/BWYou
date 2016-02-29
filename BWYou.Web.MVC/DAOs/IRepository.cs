using BWYou.Web.MVC.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.DAOs
{
    /// <summary>
    /// Repository 인터페이스
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity>
        where TEntity : BWModel
    {
        /// <summary>
        /// PK 이용 Select
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        TEntity Find(params object[] keyValues);
        /// <summary>
        /// 비동기 PK 이용 Select
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(params object[] keyValues);
        /// <summary>
        /// IQueryable 노출
        /// </summary>
        IQueryable<TEntity> Query { get; }
        /// <summary>
        /// 생성
        /// </summary>
        /// <param name="entity"></param>
        void Create(TEntity entity);
        /// <summary>
        /// PK 제외 모든 칼럼 업데이트
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        /// <summary>
        /// 관리 되지 않고 있는 TEntity의 특정 칼럼만 업데이트.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updateProperties"></param>
        void Update(TEntity entity, params string[] updateProperties);
        /// <summary>
        /// 관리 되지 않고 있는 TEntity의 값이 있는 칼럼들 모두 업데이트.
        /// </summary>
        /// <param name="entity"></param>
        void UpdateExceptNullValue(TEntity entity);
        /// <summary>
        /// 관리 되지 않고 있는 TEntity의 특정 칼럼중에서 값이 있는 칼럼들 모두 업데이트
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updateProperties"></param>
        void UpdateExceptNullValue(TEntity entity, params string[] updateProperties);
        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);
        /// <summary>
        /// 다시 불러오기
        /// </summary>
        /// <param name="entity"></param>
        void Reload(TEntity entity);
        /// <summary>
        /// Deep 복사
        /// </summary>
        /// <param name="source"></param>
        void Clone(TEntity source);
        /// <summary>
        /// DBSet 노출
        /// </summary>
        DbSet<TEntity> DBSet
        {
            get;
        }
        /// <summary>
        /// DbContext 노출
        /// </summary>
        DbContext DBContext
        {
            get;
        }
    }
}
