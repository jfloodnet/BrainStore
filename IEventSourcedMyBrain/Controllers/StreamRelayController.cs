using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IEventSourcedMyBrain.Controllers
{
    public class StreamRelayController : ApiController
    {
        public async Task<HttpResponseMessage> Get()
        {
            var response = await (new RelayService(RelayUri).Relay(Request));
            return await new LinkInterceptor(
                Config.Host, 
                Config.Port, 
                Request.RequestUri,
                GlobalConfiguration.Configuration).Intercept(response);
        }

        private Uri RelayUri(Uri uri)
        {
            var ub = new UriBuilder(uri);
            ub.Host = Config.Host;
            ub.Port = Config.Port;
            ub.Query = "format=json";
            return ub.Uri;
        }
    }
}