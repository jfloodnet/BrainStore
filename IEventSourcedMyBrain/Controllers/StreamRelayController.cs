using EventStore.ClientAPI;
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
    public class EmotivSessionsController : ApiController
    {
        public IEnumerable<EmotivSession> Get()
        {
            using (var connection = EventStoreConnection.Create())
            {
                connection.Connect(new IPEndPoint(IPAddress.Parse(Config.Host), 1113));   

                var reader = new EventStreamReader(connection);
                return reader.ReadAll<EmotivSession>("EmoSessionSummaries").ToList();
            }          
        }
    }

    public class EventStreamReader
    {
        private const int ReadPageSize = 500;

        readonly EventStoreConnection connection;

        public EventStreamReader(EventStoreConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<TEvent> ReadAll<TEvent>(string streamName)
        {
            StreamEventsSlice currentSlice;
            var nextSliceStart = 1;
            do
            {
                currentSlice = this.connection.ReadStreamEventsForward(streamName, nextSliceStart, ReadPageSize, false);
                nextSliceStart = currentSlice.NextEventNumber;

                foreach (var evnt in currentSlice.Events)
                    yield return DeserializeEvent<TEvent>(evnt.OriginalEvent.Data);
            } while (!currentSlice.IsEndOfStream);
        }

        public TEvent DeserializeEvent<TEvent>(byte[] data)
        {
            return JsonConvert.DeserializeObject<TEvent>(data.ReadAsString());
        } 
    }

    public static class ByteArrayExtensions
    {
        public static string ReadAsString(this byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public static dynamic AsDynamicJson(this string json)
        {
            return JObject.Parse(json);
        }
    }

    public class EmotivSession
    {
        public string StreamId { get; set; }
        public string UserActivity { get; set; }
        public string SessionStarted { get; set; }
        public string SessionEnded { get; set; }
        public string DisplayName
        {
            get
            {
                return SessionStarted + " - " + SessionEnded;
            }
        }
    }

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

    public class Config
    {
        public static string Host
        {            
            get
            {
                return ConfigurationManager.AppSettings["EventStore.Host"];
            }
        }

        public static int Port
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["EventStore.Port"]);
            }
        }
    }

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