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
        public Task SubscribeTo(string streamName)
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
        private static ConcurrentDictionary<string, CancellationTokenSource> cancellationTokens = new ConcurrentDictionary<string, CancellationTokenSource>();

        public HistoricalEmotivSessionHub(HistoricalEmotivSessionReader reader)
        {
            this.reader = reader;
        }

        public Task SubscribeTo(string streamName)
        {
            var ts = new CancellationTokenSource();
            CancellationToken token = ts.Token;
            cancellationTokens.TryAdd(Context.ConnectionId, ts);
            return this.reader.StartReading(streamName, Context.ConnectionId, token);
        }

        public Task Unsubscribe()
        {
            return new Task(() => {/*noopfornow*/});
        }

        public override Task OnDisconnected()
        {
            CancellationTokenSource cts;
            if (cancellationTokens.TryGetValue(Context.ConnectionId, out cts))
            {
                cts.Cancel();
            }
            return base.OnDisconnected();
        }
    }

    public class HistoricalEmotivSessionReader
    {
        private readonly EventStoreReader reader;
        
        public HistoricalEmotivSessionReader(EventStoreReader reader)
        {
            this.reader = reader;
        }

        public Task StartReading(string streamName, string connectionId, CancellationToken token)
        {
            return Task.Run(() =>
                {
                    foreach (var evnt in this.reader.ReadAll(streamName))
                    {
                        if (token.IsCancellationRequested) break;

                        SendToClient(evnt, connectionId);
                        Thread.Sleep(1000);
                    }
                }, token);
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