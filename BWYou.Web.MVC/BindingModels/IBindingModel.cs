using BWYou.Web.MVC.Models;

namespace BWYou.Web.MVC.BindingModels
{
    public interface IBindingModel<TEntity>
        where TEntity : IDbModel
    {
        TEntity CreateBaseModel();
    }


}
