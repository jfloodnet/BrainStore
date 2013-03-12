using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IEventSourcedMyBrain.Controllers
{
    public class RelayService
    {
        readonly Func<Uri, Uri> relayUri;

        public RelayService(Func<Uri, Uri> relayUri)
        {
            this.relayUri = relayUri;
        }

        public async Task<HttpResponseMessage> Relay(HttpRequestMessage message)
        {
            using (var client = new HttpClient())
            {
                return await client.GetAsync(relayUri(message.RequestUri));
            }
        }
    }
}