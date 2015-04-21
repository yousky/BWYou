using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DbFieldMapAttribute : Attribute
    {
        public string Field { get; set; }
        public DbFieldMapAttribute(string field)
        {
            Field = field;
        }
    }
}
