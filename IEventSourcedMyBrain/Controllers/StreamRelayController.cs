using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IEventSourcedMyBrain.Controllers
{
    public class StreamRelayController : ApiController
    {
        private readonly RelayService relayService;
        private readonly LinkInterceptor linkInterceptor;

        public StreamRelayController(RelayService relayService, LinkInterceptor linkInterceptor)
        {
            this.relayService = relayService;
            this.linkInterceptor = linkInterceptor;
        }

        public async Task<HttpResponseMessage> Get()
        {
            var response = await relayService.Relay(Request);
            return await linkInterceptor.Intercept(response, Request.RequestUri);
        }
    }
}