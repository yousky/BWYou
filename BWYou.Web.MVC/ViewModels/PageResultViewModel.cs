using PagedList;
using System.Collections.Generic;

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
