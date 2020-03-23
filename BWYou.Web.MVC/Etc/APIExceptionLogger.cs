using log4net;
using System;
using System.Globalization;
using System.Web.Http.ExceptionHandling;

namespace BWYou.Web.MVC.Etc
{
    public class APIExceptionLogger : ExceptionLogger
    {
        public ILog logger = LogManager.GetLogger(typeof(APIExceptionLogger));

        public override void Log(ExceptionLoggerContext context)
        {
            try
            {
                if (context.Exception != null)
                {
                    try
                    {
                        string RequestMethod =
                            (context.ExceptionContext.Request != null) ?
                                ((context.ExceptionContext.Request.Method != null) ?
                                    context.ExceptionContext.Request.Method.ToString()
                                    : "RequestMethod==null")
                                : "Request==null";

                        string ResponseStatusCode =
                            (context.ExceptionContext.Response != null) ?
                                context.ExceptionContext.Response.StatusCode.ToString()
                                : "Response==null";

                        string RequestRequestUri =
                            (context.ExceptionContext.Request != null) ?
                                ((context.ExceptionContext.Request.RequestUri != null) ?
                                    context.ExceptionContext.Request.RequestUri.ToString()
                                    : "RequestRequestUri==null")
                                : "Request==null";

                        string ControllerType =
                            (context.ExceptionContext.ControllerContext.Controller != null) ?
                                context.ExceptionContext.ControllerContext.Controller.GetType().Name
                                : "Controller==null";

                        var message = string.Format(CultureInfo.InvariantCulture,
                                            "Exception occured {0} {1} {2}\t[{3}] => {4} \r\n {5}",
                                            RequestMethod,
                                            ResponseStatusCode,
                                            RequestRequestUri,
                                            ControllerType,
                                            context.Exception.Message,
                                            context.Exception.StackTrace);

                        logger.Error(message, context.Exception);
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format(CultureInfo.InvariantCulture,
                                            "Exception occured inner {0} \r\n {1} \r\n {2} \r\n {3}",
                                            context.Exception.Message,
                                            context.Exception.StackTrace,
                                            ex.Message,
                                            ex.StackTrace);

                        logger.Error(message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                                            "Critical Log Exception occured inner inner {0} \r\n {1}",
                                            ex.Message,
                                            ex.StackTrace);

                logger.Error(message, ex);
            }
        }
    }
}
