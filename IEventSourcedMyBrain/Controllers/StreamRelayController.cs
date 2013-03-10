using EventStore.ClientAPI;
using IEventSourcedMyBrain;
using IEventSourcedMyBrain.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace ConsciousnessStream.EventStore.Relay.Controllers
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