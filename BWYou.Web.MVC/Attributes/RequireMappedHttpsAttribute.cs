using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BWYou.Web.MVC.Attributes
{
    public class RequireMappedHttpsAttribute : RequireHttpsAttribute
    {
        public readonly int HttpsPort;
        public readonly bool Permanent;

        public RequireMappedHttpsAttribute(int HttpsPort = 443, bool Permanent = false)
        {
            this.HttpsPort = HttpsPort;
            this.Permanent = Permanent;
        }

        protected override void HandleNonHttpsRequest(AuthorizationContext filterContext)
        {
            base.HandleNonHttpsRequest(filterContext);

            var result = (RedirectResult)filterContext.Result;

            var uri = new UriBuilder(result.Url);
            uri.Port = HttpsPort;

            filterContext.Result = new RedirectResult(uri.ToString(), permanent: Permanent);
        }
    }
}
