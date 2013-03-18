using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace IEventSourcedMyBrain.Hubs
{
    public class LiveEmotivSessionHub : Hub
    {
        public Task SubscribeTo(string streamName)
        {
            return Groups.Add(Context.ConnectionId, "LiveEmotivSession");
        }

        public Task Unsubscribe()
        {
            return Groups.Remove(Context.ConnectionId, "LiveEmotivSession");
        }
    }
}