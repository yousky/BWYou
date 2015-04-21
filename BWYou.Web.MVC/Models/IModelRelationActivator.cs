using System.Collections.Generic;

namespace BWYou.Web.MVC.Models
{
    public interface IModelRelationActivator
    {
        void ActivateRelation4Cascade();
        void ActivateRelation4Cascade(HashSet<object> seen);
    }
}
