using BWYou.Web.MVC.Attributes;
using BWYou.Web.MVC.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BWYou.Web.MVC.Models
{
    public interface ICUModel
    {
        DateTime? CreateDT { get; set; }
        DateTime? UpdateDT { get; set; }
    }


}
