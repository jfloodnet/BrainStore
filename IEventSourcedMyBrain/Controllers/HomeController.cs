using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Messages;
using Microsoft.AspNet.SignalR;

namespace IEventSourcedMyBrain.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

    public class LiveController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

    public class LiveEmotivSessionHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.Handle(message);
        }

        public Task Subscribe()
        {
            return Groups.Add(Context.ConnectionId, "LiveEmotivSession");
        }

        public Task Unsubscribe()
        {
            return Groups.Remove(Context.ConnectionId, "LiveEmotivSession");
        }
    }

    public class LiveEmotivSessionSubscriber
    {
        private readonly EventStoreConnection connection;

        public LiveEmotivSessionSubscriber(EventStoreConnection connection)
        {
            this.connection = connection;
        }

        public void Subscribe()
        {
            this.connection.SubscribeToAll(true, SendToClient);
        }

        private static void SendToClient(ResolvedEvent e)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<LiveEmotivSessionHub>();
            var theGroup = context.Clients.Group("LiveEmotivSession");
            var data = Encoding.UTF8.GetString(e.OriginalEvent.Data);
            var metadata = Encoding.UTF8.GetString(e.OriginalEvent.Metadata);

            theGroup.handleEvent(new
            {
                eventId = e.OriginalEvent.EventId,
                eventNumber = e.OriginalEvent.EventNumber,
                eventStreamId = e.OriginalEvent.EventStreamId,
                eventType = e.OriginalEvent.EventType,
                data,
                metadata
            });
        }
    }
}
