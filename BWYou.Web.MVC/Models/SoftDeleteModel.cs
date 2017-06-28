using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.Models
{
    public class SoftDeleteModel<TId> : BWModel<TId>
    {
        public bool? IsDeleted { get; set; }

        public SoftDeleteModel()
            : base()
        {
            IsDeleted = false;
        }
    }
}
