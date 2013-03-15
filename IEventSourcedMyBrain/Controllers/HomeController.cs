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

        public void SubscribeToThis()
        {
            Groups.Add(Context.ConnectionId, "LiveEmotivSession");
        }

        public void Unsubscribe()
        {
            Groups.Remove(Context.ConnectionId, "LiveEmotivSession");
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
            this.connection.SubscribeToStream("$ce-EmoSession", true, SendToClient);
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
