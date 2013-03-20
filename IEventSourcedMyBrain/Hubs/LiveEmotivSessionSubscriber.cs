using System.Text;
using EventStore.ClientAPI;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace IEventSourcedMyBrain.Hubs
{
    public class LiveEmotivSessionSubscriber
    {
        private readonly EventStoreConnection connection;
        
        public LiveEmotivSessionSubscriber(EventStoreConnection connection)
        {
            this.connection = connection;
        }

        public Task<EventStoreSubscription> Subscribe(string connectionId)
        {
            return this.connection.SubscribeToStream("$ce-EmoSession", true, e => SendToClient(e, connectionId));
        }

        private static void SendToClient(ResolvedEvent e, string connectionId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<LiveEmotivSessionHub>();
            var theClient = context.Clients.Client(connectionId);
            var data = Encoding.UTF8.GetString(e.Event.Data);
            var metadata = Encoding.UTF8.GetString(e.Event.Metadata);

            theClient.handleEvent(new
                {
                    eventId = e.Event.EventId,
                    eventNumber = e.Event.EventNumber,
                    eventStreamId = e.Event.EventStreamId,
                    eventType = e.Event.EventType,
                    data,
                    metadata
                });
        }
    }
}