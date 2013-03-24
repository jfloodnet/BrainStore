using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace ConsciousnessStream.Extensions
{
    public static class DictionaryExtensions
    {
        public static byte[] ToByteArray(this Dictionary<string, object> eventHeaders)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, SerializerSettings));
        }

        private static readonly JsonSerializerSettings SerializerSettings;

        static DictionaryExtensions()
        {
            SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
        }
    }
}
