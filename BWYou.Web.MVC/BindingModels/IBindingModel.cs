using BWYou.Web.MVC.Models;

namespace BWYou.Web.MVC.BindingModels
{
    public interface IBindingModel<TEntity>
        where TEntity : BWModel
    {
        TEntity CreateBaseModel();
    }


}
