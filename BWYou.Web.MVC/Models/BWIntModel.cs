using BWYou.Web.MVC.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BWYou.Web.MVC.Models
{
    //TODO BWModel은 TId generic으로 하고, int?용 모델 새로 만들어서 새 모델용으로 서비스, 컨트롤러 변경 필요
    public class BWIntModel : BWModel<int?>
    {

    }
}
