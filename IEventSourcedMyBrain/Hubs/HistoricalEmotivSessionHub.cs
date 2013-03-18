using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace IEventSourcedMyBrain.Hubs
{
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
}