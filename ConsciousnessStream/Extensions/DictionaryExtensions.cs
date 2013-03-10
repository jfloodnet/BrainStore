using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
