using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.ViewModels
{
    public class ValidationObjectError
    {
        public string ObjectName { get; set; }
        public string ErrorMessage { get; set; }

        public ValidationObjectError(string ObjectName, string ErrorMessage)
        {
            this.ObjectName = ObjectName;
            this.ErrorMessage = ErrorMessage;
        }
    }
}
