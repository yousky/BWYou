using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.ModelBinding;

namespace BWYou.Web.MVC.ViewModels
{
    public class ErrorResultViewModel
    {
        public WebStatusMessageBody Error { get; set; }
        public List<ValidationObjectError> ModelValidResult { get; set; }

        public ErrorResultViewModel(HttpStatusCode httpStatusCode, string message)
        {
            Error = new WebStatusMessageBody()
            {
                Status = (int)httpStatusCode,
                Code = "E" + string.Format("{0:D3}", (int)httpStatusCode),
                Message = message,
                Link = "",
                DeveloperMessage = ""
            };
        }

        public ErrorResultViewModel(HttpStatusCode httpStatusCode, ExceptionHandlerContext context)
        {
            Error = new WebStatusMessageBody()
            {
                Status = (int)httpStatusCode,
                Code = "E500",
                Message = context.Exception.Message,
                Link = "",
#if(!DEBUG)
                DeveloperMessage = ""
#else
                DeveloperMessage = context.Exception.ToString()
#endif
            };
        }

        public ErrorResultViewModel(HttpStatusCode httpStatusCode, ModelStateDictionary modelState)
        {
            Error = new WebStatusMessageBody()
            {
                Status = (int)httpStatusCode,
                Code = "E400",
                Message = "Validation Fail",
                Link = "",
#if(!DEBUG)
                DeveloperMessage = ""
#else
                DeveloperMessage = "Validation Fail"
#endif
            };
            ModelValidResult = GetValidationObjectErrors(modelState);
        }

        protected List<ValidationObjectError> GetValidationObjectErrors(ModelStateDictionary modelState)
        {
            List<ValidationObjectError> validationObjectErrors = new List<ValidationObjectError>();
            foreach (var key in modelState.Keys)
            {
                foreach (var e in modelState[key].Errors)
                {
                    validationObjectErrors.Add(new ValidationObjectError(key, e.ErrorMessage));
                }
            }

            return validationObjectErrors;
        }
    }
}
