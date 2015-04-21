using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.Attributes
{
    /// <summary>
    /// DB에서 업데이트 될 수 있는 칼럼임을 나타냄
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UpdatableAttribute : Attribute
    {
    }
}
