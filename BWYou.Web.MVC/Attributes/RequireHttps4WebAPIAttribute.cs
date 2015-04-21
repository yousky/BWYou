using BWYou.Web.MVC.ViewModels;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BWYou.Web.MVC.Attributes
{
    public class RequireHttps4WebAPIAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, "RequireHttps"));
            }
        }
    }
}
