using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

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
            using (HttpClient client = new HttpClient())
            {
                return await client.GetAsync(RelayUri(message.RequestUri));
            }
        }

        private Uri RelayUri(Uri uri)
        {
            UriBuilder ub = new UriBuilder(uri);
            ub.Host = host;
            ub.Port = port;
            ub.Query = "format=json";
            return ub.Uri;
        }
    }
}