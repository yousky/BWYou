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
    public class BWLongApiVMController<TEntity, TVM> : BWApiVMController<TEntity, long?, TVM>
        where TEntity : BWLongModel
        where TVM : IModelLoader<TEntity>, new()
    {
        public BWLongApiVMController(DbContext dbContext)
            : base(dbContext)
        {

        }

        public BWLongApiVMController(BWLongEntityService<TEntity> service)
            : base(service)
        {

        }
    }
}
