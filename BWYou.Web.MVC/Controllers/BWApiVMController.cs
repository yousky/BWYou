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
    public class BWApiVMController<TEntity, TId, TVM> : IdApiVMController<TEntity, TId, TVM>
        where TEntity : BWModel<TId>
        where TVM : IModelLoader<TEntity>, new()
    {
        public BWApiVMController(DbContext dbContext)
            : base(dbContext)
        {

        }

        public BWApiVMController(BWEntityService<TEntity, TId> service)
            : base(service)
        {

        }
    }
}
