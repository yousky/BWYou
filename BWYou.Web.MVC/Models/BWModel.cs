using BWYou.Web.MVC.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BWYou.Web.MVC.Models
{
    public class BWModel
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
    }
}
