using BWYou.Web.MVC.Models;
using BWYou.Web.MVC.Services;
using BWYou.Web.MVC.ViewModels;
using System;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.ModelBinding;

namespace BWYou.Web.MVC.Controllers
{
    /// <summary>
    /// 공통으로 사용 할 수 있도록 genetic을 활용 하여 공통 모델에 대한 Restful 기능을 구현
    /// 단 여기서 직접적으로 호출 되지 않도록 함수 앞에 Base를 붙이고, protected 처리 해서 기능을 상속 받은 자식이 호출 해서 사용하도록 처리
    /// </summary>
    /// <typeparam name="TEntity">기본 모델 엔티티</typeparam>
    /// <typeparam name="TId"></typeparam>
    public class BWApiController<TEntity, TId> : IdApiController<TEntity, TId>
        where TEntity : BWModel<TId>
    {
        public BWApiController(DbContext dbContext)
            : base(dbContext)
        {

        }

        public BWApiController(BWEntityService<TEntity, TId> service)
            : base(service)
        {

        }
    }
}
