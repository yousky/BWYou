using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.Models
{
    public interface IModelRelationActivator
    {
        void ActivateRelation4Cascade();
        void ActivateRelation4Cascade(HashSet<object> seen);
    }
}
