using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BWYou.Web
{
    public class WebException : Exception
    {
        private int _statusCode = 0;

        public WebException(int code, string message)
            : base(message)
        {
            _statusCode = code;
        }

        public WebException(string message, Exception ex)
            : base(message, ex)
        {
            _statusCode = WebStatus.STATUS_SERVER_ERROR;
        }

        public int GetStatusCode()
        {
            return _statusCode;
        }
    }

    public class InvalidParamWebException : WebException
    {
        public InvalidParamWebException(string parameterName)
            : base(WebStatus.STATUS_INVALID_PARAM, "Invalid Paramter: " + parameterName)
        { }

        public InvalidParamWebException(string parameterName, string comment)
            : base(WebStatus.STATUS_INVALID_PARAM, String.Format("Invalid Paramter({0}): {1}", comment, parameterName))
        { }
    }

}
