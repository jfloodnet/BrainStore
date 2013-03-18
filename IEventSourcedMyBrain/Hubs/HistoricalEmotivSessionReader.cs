using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Concurrent;
using IEventSourcedMyBrain.Controllers;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace IEventSourcedMyBrain.Hubs
{
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