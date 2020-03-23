using log4net;
using System.Globalization;
using System.Web.Mvc;

namespace BWYou.Web.MVC.Controllers
{
    public class BWController : Controller
    {
        public ILog logger = LogManager.GetLogger(typeof(BWController));

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // If in Debug mode...
            if (filterContext.HttpContext.IsDebuggingEnabled)
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                                            "{0} {1}",
                                            filterContext.HttpContext.Request.HttpMethod,
                                            filterContext.HttpContext.Request.Url);

                logger.Debug(message);
            }

            base.OnActionExecuting(filterContext);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var message = string.Format(CultureInfo.InvariantCulture,
                                        "{0} {1} {2}\t[{3}\\{4}]",
                                        filterContext.HttpContext.Request.HttpMethod,
                                        filterContext.RequestContext.HttpContext.Response.StatusCode,
                                        filterContext.HttpContext.Request.Url,
                                        filterContext.Controller.GetType().Name,
                                        filterContext.ActionDescriptor.ActionName.Trim());

            logger.Info(message);

            // Logs error no matter what
            if (filterContext.Exception != null)
            {
                try
                {
                    message = string.Format(CultureInfo.InvariantCulture,
                                                "Exception occured {0}.{1} => {2} \r\n {3}",
                                                filterContext.Controller.GetType().Name,
                                                filterContext.ActionDescriptor.ActionName.Trim(),
                                                filterContext.Exception.Message,
                                                filterContext.Exception.StackTrace);
                    logger.Error(message, filterContext.Exception);
                }
                catch (System.Exception ex)
                {
                    message = string.Format(CultureInfo.InvariantCulture,
                                        "Exception occured inner {0} \r\n {1} \r\n {2} \r\n {3}",
                                        filterContext.Exception.Message,
                                        filterContext.Exception.StackTrace,
                                        ex.Message,
                                        ex.StackTrace);

                    logger.Error(message, ex);
                }
            }

            base.OnActionExecuted(filterContext);
        }

        protected void WriteLog4Client(string message, string logLevel = "INFO")
        {
            string REMOTEADDR = Request.ServerVariables["REMOTE_ADDR"];
            string REMOTEHOST = Request.ServerVariables["REMOTE_HOST"];
            string XRealIP = Request.ServerVariables["X-Real-IP"];
            string UserHostAddress = Request.UserHostAddress;
            message = string.Format(CultureInfo.InvariantCulture, "REMOTE_ADDR={0}, X-Real-IP={1}, UserHostAddress={2}, REMOTE_HOST={3} : {4}", REMOTEADDR, XRealIP, UserHostAddress, REMOTEHOST, message);
            if (logLevel.ToUpper() == "INFO")
            {
                logger.Info(message);
            }
            else if (logLevel.ToUpper() == "DEBUG")
            {
                logger.Debug(message);
            }
            else if (logLevel.ToUpper() == "WARN")
            {
                logger.Warn(message);
            }
            else if (logLevel.ToUpper() == "ERROR")
            {
                logger.Error(message);
            }
            else if (logLevel.ToUpper() == "FATAL")
            {
                logger.Fatal(message);
            }
            else
            {
                logger.Debug(message);
            }
        }
    }
}
