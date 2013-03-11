using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IEventSourcedMyBrain.Controllers
{
    public class StreamRelayController : ApiController
    {
        public async Task<HttpResponseMessage> Get()
        {
            var response = await (new RelayService(Config.Host, Config.Port).Relay(Request));
            return await new LinkInterceptor(
                Config.Host, 
                Config.Port, 
                Request.RequestUri,
                GlobalConfiguration.Configuration).Intercept(response);
        }
    }
}