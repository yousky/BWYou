using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.Attributes
{
    /// <summary>
    /// DB에서 검색 시 Where 조건으로 필터링 될 수 있는 칼럼임을 나타냄
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterableAttribute : Attribute
    {
    }
}
