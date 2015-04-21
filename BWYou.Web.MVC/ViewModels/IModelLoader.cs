using BWYou.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.ViewModels
{
    public interface IModelLoader<TEntity>
        where TEntity : BWModel
    {
        void LoadModel(TEntity baseModel, bool recursive, string sort = "Id");
    }
}
