using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace IEventSourcedMyBrain.Hubs
{
    public class LiveEmotivSessionHub : Hub
    {
        private readonly object lockObject = new object();
        private int numberOfSubscribers = 0;

        private readonly LiveEmotivSessionSubscriber subscriber;

        public LiveEmotivSessionHub(LiveEmotivSessionSubscriber subscriber)
        {
            this.subscriber = subscriber;
        }

        public Task SubscribeTo(string streamName)
        {
            IfFirstOneInOpenTheDoor();
            return Groups.Add(Context.ConnectionId, "LiveEmotivSession");
        }

        private void IfFirstOneInOpenTheDoor()
        {
            lock (lockObject)
            {
                if(numberOfSubscribers == 0)
                    Task.Run(() => this.subscriber.Subscribe());
                numberOfSubscribers++;
            }
        }

        public override Task OnDisconnected()
        {
            Unsubscribe();
            return base.OnDisconnected();
        }

        public Task Unsubscribe()
        {
            IfLastOneOutShutTheDoor();
            return Groups.Remove(Context.ConnectionId, "LiveEmotivSession");
        }

        private void IfLastOneOutShutTheDoor()
        {
            lock (lockObject)
            {
                if(numberOfSubscribers == 1)
                    this.subscriber.Unsubscribe();
                numberOfSubscribers--;
            }
        }
    }
}