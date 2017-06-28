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
    public class BWLongEntityService<TEntity> : BWEntityService<TEntity, long?>
        where TEntity : BWModel<long?>
    {
        public BWLongEntityService(DbContext dbContext)
            : base(dbContext)
        {

        }

        public BWLongEntityService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

    }
}
