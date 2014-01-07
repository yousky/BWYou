using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using NLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BWYou.Web
{
    public abstract class PageBase : System.Web.UI.Page
    {
        /// <summary>
        /// Logger
        /// Fatal
        /// Error
        /// Warn
        /// Info
        /// Debug
        /// Trace
        /// Off
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        protected Logger GetLogger() { return _logger; }

        protected void WriteLog(Exception ex)
        {
            GetLogger().Error(string.Format("Exception: {0}\nStackTrace: {1}", ex.Message, ex.StackTrace));
        }

        /// <summary>
        /// 
        /// <exception cref="WebException">Request가 유효하지 않은 경우, Exception 발생.</exception>
        /// </summary>
        protected abstract void CheckValidRequest();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>JSON으로 변환할 Content 리턴</returns>
        protected abstract object GetLogicContent();

        protected void CheckPostParamExisted(string parameterName)
        {
            if (String.IsNullOrEmpty(Request.Form.Get(parameterName)))
            {
                throw new InvalidParamWebException(parameterName);
            }
        }

        protected void CheckPostParamsExisted(string[] parameterNames)
        {
            foreach (string parameterName in parameterNames)
            {
                CheckPostParamExisted(parameterName);
            }
        }

        protected bool HasPostParam(string parameterName)
        {
            if (String.IsNullOrEmpty(Request.Form.Get(parameterName)))
            {
                return false;
            }
            return true;
        }

        protected string GetPostParam(string parameterName)
        {
            if (String.IsNullOrEmpty(Request.Form.Get(parameterName)))
            {
                return null;
            }
            return Request.Form.Get(parameterName);
        }

        protected string GetPostParamNonEmptyString(string parameterName)
        {
            string result = GetPostParam(parameterName);
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new InvalidParamWebException(parameterName, "String(isEmpty)");
            }
            return result;
        }

        protected int GetPostParamInt(string parameterName)
        {
            string param = GetPostParam(parameterName);
            int result = 0;
            if (param == null || Int32.TryParse(param, out result) == false)
            {
                throw new InvalidParamWebException(parameterName, "Integer");
            }
            return result;
        }

        protected int GetPostParamIntNonZero(string parameterName)
        {
            int result = GetPostParamInt(parameterName);
            if (result == 0)
            {
                throw new InvalidParamWebException(parameterName, "Integer(isZero)");
            }
            return result;
        }

        protected long GetPostParamLong(string parameterName)
        {
            string param = GetPostParam(parameterName);
            long result = 0;
            if (param == null || long.TryParse(param, out result) == false)
            {
                throw new InvalidParamWebException(parameterName, "Long");
            }
            return result;
        }

        protected long GetPostParamLongNonZero(string parameterName)
        {
            long result = GetPostParamLong(parameterName);
            if (result == 0)
            {
                throw new InvalidParamWebException(parameterName, "Long(isZero)");
            }
            return result;
        }

        protected bool GetPostParamBoolean(string parameterName)
        {
            string param = GetPostParam(parameterName);
            if (String.IsNullOrEmpty(param))
            {
                throw new InvalidParamWebException(parameterName, "Boolean");
            }
            if (param == "0")
            {
                return false;
            }
            else if (param == "1")
            {
                return true;
            }
            throw new InvalidParamWebException(parameterName, "Boolean");
        }

        protected JObject GetPostParamJson(string parameterName)
        {
            string param = GetPostParam(parameterName);
            if (param != null)
            {
                return param.ToJson();
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";

            TraceLogRequest();

            // 유효성 체크
            int statusCode = 0;
            string errorMessage = null;
            try
            {
                CheckValidRequest();
            }
            catch (WebException ex)
            {
                statusCode = ex.GetStatusCode();
                errorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                statusCode = WebStatus.STATUS_UNKNOWN_ERROR;
                errorMessage = ex.Message + "\n" + ex.StackTrace;
            }

            object responseObject = null;
            if (statusCode != 0)
            {
                // 유효성 체크 오류시에 응답 생성.
                if (errorMessage != null)
                {
                    responseObject = new { status = statusCode, message = errorMessage };
                }
                else
                {
                    responseObject = new { status = statusCode };
                }
            }
            else
            {
                // 실제 각 페이지마다 로직 실행.
                try
                {
                    var contentObject = GetLogicContent();
                    if (contentObject != null)
                    {
                        responseObject = new { status = statusCode, content = contentObject };
                    }
                    else
                    {
                        responseObject = new { status = 0 };
                    }
                }
                catch (WebException ex)
                {
                    responseObject = new { status = ex.GetStatusCode(), message = ex.Message };
                }
                catch (Exception ex)
                {
                    responseObject = new { status = WebStatus.STATUS_SERVER_ERROR, message = ex.GetType().ToString() + ":" + ex.Message + ex.StackTrace };
                }
            }

            string responseString = JsonConvert.SerializeObject(responseObject, Formatting.None, Consts.JsonSerializerSettings);
            TraceLogResponse(responseString);
            Response.Write(responseString);
            Response.End();

            OnPostResponse();
        }

        protected virtual void TraceLogRequest()
        {
            GetLogger().Info("REQ@: {0}({1}): {2}", Request.Path, Request.UserHostAddress, JsonConvert.SerializeObject(Request.Form.ToDictionary(), Formatting.None));
        }

        protected virtual void TraceLogResponse(string responseString)
        {
            GetLogger().Info("RES#: " + responseString);
        }

        protected virtual void OnPostResponse()
        {
        }

        protected void Application_Error()
        {
            GetLogger().Fatal(Server.GetLastError());
        }

        protected void RunOnBackground(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }


            BackgroundWorker backgroundWorker = (BackgroundWorker)Application["worker"];
            if (backgroundWorker != null)
            {
                GetLogger().Info("RunOnBackground()");
                backgroundWorker.AddWork(action);
            }
        }
    }
}
