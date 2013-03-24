using Newtonsoft.Json.Linq;
using System.Text;

namespace IEventSourcedMyBrain.Extensions
{
    public static class Extensions
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