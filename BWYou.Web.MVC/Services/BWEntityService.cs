﻿using BWYou.Web.MVC.Attributes;
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
    public class BWEntityService<TEntity, TId> : IdEntityService<TEntity, TId>
        where TEntity : BWModel<TId>
    {
        public BWEntityService(DbContext dbContext)
            : base(dbContext)
        {

        }

        public BWEntityService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

    }
}
