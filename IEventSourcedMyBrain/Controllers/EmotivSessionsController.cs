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
        private readonly EventStoreReader reader;

        public EmotivSessionsController(EventStoreReader reader)
        {
            this.reader = reader;
        }

        public IEnumerable<EmotivSession> Get()
        {
            return reader.ReadAll("EmoSessionSummaries").As<EmotivSession>();
        }        
    }
}
