using BWYou.Web.MVC.Models;

namespace BWYou.Web.MVC.ViewModels
{
    public interface IModelLoader<TEntity>
        where TEntity : BWModel
    {
        void LoadModel(TEntity baseModel, bool recursive, string sort = "Id");
    }
}
