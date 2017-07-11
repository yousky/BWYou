using BWYou.Web.MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.Exceptions
{
    public class ErrorResultVMException : Exception
    {
        public ErrorResultViewModel ErrorResultViewModel { get; set; }

        public ErrorResultVMException() : base()
        {

        }
        public ErrorResultVMException(string message)
            : base(message)
        {

        }
        public ErrorResultVMException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
