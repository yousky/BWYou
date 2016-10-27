using BWYou.Web.MVC.Models;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.ViewModels
{
    public interface IModelLoader<TEntity>
        where TEntity : IDbModel
    {
        void LoadModel(TEntity baseModel, int curDepth = 0, int targetDepth = 0, string sort = "Id");
        Task LoadModelAsync(TEntity baseModel, int curDepth = 0, int targetDepth = 0, string sort = "Id");
    }


}
