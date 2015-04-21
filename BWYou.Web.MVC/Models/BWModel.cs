using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BWYou.Web.MVC.Attributes;

namespace BWYou.Web.MVC.Models
{
    public class BWModel : IModelRelationActivator
    {
        [Key]
        [Display(Name = "ID")]
        [Filterable]
        public virtual int? Id { get; set; }
        [Display(Name = "생성된 날짜")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public virtual DateTime? CreateDT { get; set; }
        [Display(Name = "수정된 날짜")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public virtual DateTime? UpdateDT { get; set; }

        public BWModel()
        {
            CreateDT = DateTime.Now;
            UpdateDT = DateTime.Now;
        }

        /// <summary>
        /// 관계 정보를 모두 불러와서 활성화 시키기 시작
        /// </summary>
        public void ActivateRelation4Cascade()
        {
            HashSet<object> seen = new HashSet<object>();
            ActivateRelation4Cascade(seen);
        }

        public virtual void ActivateRelation4Cascade(HashSet<object> seen)
        {
            //무한 재귀 안 되도록 확인
            if (seen.Contains(this) == true)
            {
                return;
            }
            else
            {
                seen.Add(this);
            }
        }

        protected void ActivateRelation4Cascade(ICollection<IModelRelationActivator> collection, HashSet<object> seen)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    item.ActivateRelation4Cascade(seen);
                }
            }
        }

        protected void ActivateRelation4Cascade(IModelRelationActivator model, HashSet<object> seen)
        {
            if (model != null)
            {
                model.ActivateRelation4Cascade(seen);
            }
        }
    }
}
