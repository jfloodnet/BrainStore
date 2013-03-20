using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.AspNet.SignalR;

namespace IEventSourcedMyBrain.Hubs
{
    public class LiveEmotivSessionHub : Hub
    {
        private readonly LiveEmotivSessionSubscriber subscriber;

        private readonly static ConcurrentDictionary<string, EventStoreSubscription> subscriptions = new ConcurrentDictionary<string, EventStoreSubscription>();

        public LiveEmotivSessionHub(LiveEmotivSessionSubscriber subscriber)
        {
            this.subscriber = subscriber;
        }

        public async Task SubscribeTo(string streamName)
        {
            TryCancelCurrentSubscriptionForConnection();
            var subscription = await this.subscriber.Subscribe(Context.ConnectionId);
            if (subscriptions.TryAdd(Context.ConnectionId, subscription))
            {
                //noop
            }
        }
        
        public override Task OnDisconnected()
        {
            TryCancelCurrentSubscriptionForConnection();
            return base.OnDisconnected();
        }

        public void Unsubscribe()
        {
            TryCancelCurrentSubscriptionForConnection();
        }

        private void TryCancelCurrentSubscriptionForConnection()
        {
            EventStoreSubscription subscription;
            if (subscriptions.TryRemove(Context.ConnectionId, out subscription))
            {
                subscription.Unsubscribe();
            }
        }
    }
}