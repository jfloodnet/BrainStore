using EventStore.ClientAPI;
using IEventSourcedMyBrain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using IEventSourcedMyBrain.Extensions;

namespace IEventSourcedMyBrain.Controllers
{
    public class EmotivSessionsController : ApiController
    {
        public IEnumerable<EmotivSession> Get()
        {
            using (var connection = EventStoreConnection.Create())
            {
                connection.Connect(new IPEndPoint(IPAddress.Parse(Config.Host), 1113));

                var reader = new EventStoreReader(connection);
                return reader.ReadAll<EmotivSession>("EmoSessionSummaries").ToList();
            }
        }
    }
}
