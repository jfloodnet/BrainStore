using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IEventSourcedMyBrain.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ReadAsString(this byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public static dynamic AsDynamicJson(this string json)
        {
            return JObject.Parse(json);
        }
    }
}