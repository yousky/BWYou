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
    public class BWStringApiVMController<TEntity, TVM> : BWApiVMController<TEntity, string, TVM>
        where TEntity : BWStringModel
        where TVM : IModelLoader<TEntity>, new()
    {
        public BWStringApiVMController(DbContext dbContext)
            : base(dbContext)
        {

        }

        public BWStringApiVMController(BWStringEntityService<TEntity> service)
            : base(service)
        {

        }
    }
}
