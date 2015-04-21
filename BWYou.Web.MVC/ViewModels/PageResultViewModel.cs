using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace BWYou.Web.MVC.ViewModels
{
    public class PageResultViewModel<T>
    {
        public IEnumerable<T> Result { get; set; }
        public MetaData MetaData { get; set; }

        public PageResultViewModel(IEnumerable<T> result)
        {
            this.Result = result;
        }
        public PageResultViewModel(IEnumerable<T> result, MetaData MetaData)
        {
            this.Result = result;
            this.MetaData = MetaData;
        }

        public PageResultViewModel(IPagedList<T> result)
        {
            this.Result = result;
            this.MetaData = new MetaData(result);
        }
    }
}
