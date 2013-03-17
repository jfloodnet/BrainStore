using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Concurrent;
using IEventSourcedMyBrain.Controllers;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace IEventSourcedMyBrain.Hubs
{
    public class LiveEmotivSessionHub : Hub
    {
        public void SubscribeTo(string streamName)
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

    public class HistoricalEmotivSessionHub : Hub
    {
        private HistoricalEmotivSessionReader reader;

        public HistoricalEmotivSessionHub()
        {
            this.reader = (HistoricalEmotivSessionReader)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(HistoricalEmotivSessionReader));
        }

        public void SubscribeTo(string streamName)
        {
            Task.Run(() => this.reader.StartReading(streamName, Context.ConnectionId));                       
        }

        public void Unsubscribe()
        {
            
        }
    }

    public class HistoricalEmotivSessionReader
    {
        private readonly EventStoreReader reader;

        public HistoricalEmotivSessionReader(EventStoreReader reader)
        {
            this.reader = reader;
        }

        public void StartReading(string streamName, string connectionId)
        {
            foreach (var evnt in this.reader.ReadAll(streamName))
            {
                SendToClient(evnt, connectionId);
                Thread.Sleep(1000);
            }
        }

        private static void SendToClient(ResolvedEvent e, string connectionId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<HistoricalEmotivSessionHub>();

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