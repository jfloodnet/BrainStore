using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IEventSourcedMyBrain.Controllers
{
    public class RelayService
    {
        readonly string host;
        readonly int port;

        public RelayService(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public async Task<HttpResponseMessage> Relay(HttpRequestMessage message)
        {
            using (var client = new HttpClient())
            {
                return await client.GetAsync(RelayUri(message.RequestUri));
            }
        }

        private Uri RelayUri(Uri uri)
        {
            var ub = new UriBuilder(uri);
            ub.Host = host;
            ub.Port = port;
            ub.Query = "format=json";
            return ub.Uri;
        }
    }
}