using BWYou.Web.MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace BWYou.Web.MVC.Etc
{
    public class APIExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new WebStatusMessageBodyErrorResult
            {
                Request = context.ExceptionContext.Request,
                Content = new ErrorResultViewModel(HttpStatusCode.InternalServerError, context)
            };
        }

        private class WebStatusMessageBodyErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public ErrorResultViewModel Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response =
                                 new HttpResponseMessage((HttpStatusCode)Content.Error.Status);
                response.Content = new ObjectContent(Content.GetType(), Content, new JsonMediaTypeFormatter());
                response.RequestMessage = Request;
                return Task.FromResult(response);
            }
        }
    }
}
