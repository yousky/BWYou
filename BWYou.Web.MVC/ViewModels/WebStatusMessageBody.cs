using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.ViewModels
{
    public class WebStatusMessageBody
    {
        public int Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public string DeveloperMessage { get; set; }

        public WebStatusMessageBody()
        {

        }
    }
}
