using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace IEventSourcedMyBrain.Controllers
{
    public class LinkInterceptor
    {
        readonly string host;
        readonly int port;
        readonly Uri requestUri;
        readonly HttpConfiguration cfg;

        public LinkInterceptor(string host, int port, Uri requestUri, HttpConfiguration cfg)
        {
            this.host = host;
            this.port = port;
            this.requestUri = requestUri;
            this.cfg = cfg;
        }

        public async Task<HttpResponseMessage> Intercept(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            var builder = new UriBuilder(requestUri);
            builder.Path = builder.Query = null;

            string pattern = "http://" + host + ":" + port + "/";
            string responseWithInterceptedLinks = json.Replace(pattern, builder.ToString());

            var responsejson = JObject.Parse(responseWithInterceptedLinks);

            response.Content =
                new ObjectContent<object>(responsejson,
                    cfg.Formatters.JsonFormatter, new MediaTypeHeaderValue("application/json"));

            return response;
        }
    }
}