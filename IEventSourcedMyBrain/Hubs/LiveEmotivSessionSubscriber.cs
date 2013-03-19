using System.Text;
using EventStore.ClientAPI;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace IEventSourcedMyBrain.Hubs
{
    public class LiveEmotivSessionSubscriber
    {
        private readonly EventStoreConnection connection;
        private EventStoreSubscription subscription;

        public LiveEmotivSessionSubscriber(EventStoreConnection connection)
        {
            this.connection = connection;
        }

        public async Task Subscribe()
        {
            this.subscription = await this.connection.SubscribeToStream("$ce-EmoSession", true, SendToClient);
        }

        public void Unsubscribe()
        {
            if (subscription != null) 
                this.subscription.Unsubscribe();
        }

        private static void SendToClient(ResolvedEvent e)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<LiveEmotivSessionHub>();
            var theGroup = context.Clients.Group("LiveEmotivSession");
            var data = Encoding.UTF8.GetString(e.Event.Data);
            var metadata = Encoding.UTF8.GetString(e.Event.Metadata);

            theGroup.handleEvent(new
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