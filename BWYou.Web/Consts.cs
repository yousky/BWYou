using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BWYou.Web
{
    public static class Consts
    {
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>() { 
                    new IsoDateTimeConverter() { 
                        DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ssK",
                        DateTimeStyles = (System.Globalization.DateTimeStyles.AssumeLocal | System.Globalization.DateTimeStyles.AdjustToUniversal)
                    } 
                }
        };

    }
}
