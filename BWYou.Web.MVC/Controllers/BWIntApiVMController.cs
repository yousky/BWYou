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
    public class BWIntApiVMController<TEntity, TVM> : BWApiVMController<TEntity, int?, TVM>
        where TEntity : BWModel<int?>
        where TVM : IModelLoader<TEntity>, new()
    {
        public BWIntApiVMController(DbContext dbContext)
            : base(dbContext)
        {

        }

        public BWIntApiVMController(BWIntEntityService<TEntity> service)
            : base(service)
        {

        }
    }
}
