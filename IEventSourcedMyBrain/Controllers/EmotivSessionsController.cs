using EventStore.ClientAPI;
using IEventSourcedMyBrain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

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
