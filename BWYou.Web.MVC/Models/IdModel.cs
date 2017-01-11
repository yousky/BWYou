using BWYou.Web.MVC.Attributes;
using BWYou.Web.MVC.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BWYou.Web.MVC.Models
{
    public class IdModel<TId> : IIdModel<TId>
    {
        [Key]
        [Display(Name = "ID")]
        [Filterable(false)]
        public virtual TId Id { get; set; }

        string IDbModel.ToString()
        {
            return Id.ToString();
        }

        public string GetKeyName()
        {
            return "Id";
        }
    }


}
