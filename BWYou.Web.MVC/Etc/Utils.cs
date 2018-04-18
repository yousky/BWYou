using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace BWYou.Web.MVC.Etc
{
    public class Utils
    {
        /// <summary>
        /// Revalidation for Web Api
        /// http://stackoverflow.com/questions/12906359/revalidate-model-when-using-webapi-tryvalidatemode-equivalent
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool TryValidateModel(ModelStateDictionary modelState, object model)
        {
            return TryValidateModel(modelState, model, "" /* prefix */);
        }
        /// <summary>
        /// Revalidation for Web Api
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static bool TryValidateModel(ModelStateDictionary modelState, object model, string prefix = "")
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            bool isValid = true;
            var context = new ValidationContext(model, null, null);
            var vrs = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, vrs, true);
            foreach (var vr in vrs)
            {
                foreach (var member in vr.MemberNames)
                {
                    modelState.AddModelError(prefix + member, vr.ErrorMessage);
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
