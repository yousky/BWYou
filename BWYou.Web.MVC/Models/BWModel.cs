using BWYou.Web.MVC.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BWYou.Web.MVC.Models
{
    public class BWModel<TId> : IdModel<TId>, ICUModel
    {
        [Filterable(false)]
        [Display(Name = "Created DateTime")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public virtual DateTime? CreateDT { get; set; }
        [Filterable(false)]
        [Display(Name = "Updated DateTime")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public virtual DateTime? UpdateDT { get; set; }

        public BWModel()
        {
            CreateDT = DateTime.Now;
            UpdateDT = DateTime.Now;
        }
    }
}
