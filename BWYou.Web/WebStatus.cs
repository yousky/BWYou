using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BWYou.Web
{
    public class WebStatus
    {
        public const int STATUS_OK = 0;
        public const int STATUS_INVALID_PARAM = 1;
        public const int STATUS_NOT_EXISTED_SESSION_ID = 2;
        public const int STATUS_EXPIRED_SESSION_ID = 3;
        public const int STATUS_INVALID_SESSION_ID = 4;
        public const int STATUS_SERVER_ERROR = 98;
        public const int STATUS_UNKNOWN_ERROR = 99;
    }
}
