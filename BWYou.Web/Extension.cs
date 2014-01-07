using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BWYou.Web
{
    public static class Extension
    {
        public static JObject ToJson(this string str)
        {
            try
            {
                return JObject.Parse(str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IDictionary<string, string> ToDictionary(this NameValueCollection source)
        {
            return source.Cast<string>()
                         .Select(s => new { Key = s, Value = source[s] })
                         .ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
