using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace IEventSourcedMyBrain.Hubs
{
    public class HistoricalEmotivSessionHub : Hub
    {
        private readonly HistoricalEmotivSessionReader reader;
        private readonly Task empty = Task.FromResult(0);

        private readonly static ConcurrentDictionary<string, CancellationTokenSource> cancellationTokens = new ConcurrentDictionary<string, CancellationTokenSource>();

        public HistoricalEmotivSessionHub(HistoricalEmotivSessionReader reader)
        {
            this.reader = reader;
        }

        public Task SubscribeTo(string streamName)
        {
            TryCancelCurrentReadingForConnection();

            var source  = new CancellationTokenSource();
            if(cancellationTokens.TryAdd(Context.ConnectionId, source))
                return this.reader.StartReading(streamName, Context.ConnectionId, source.Token);

            return empty;
        }

        public void Unsubscribe(string streamName)
        {
            TryCancelCurrentReadingForConnection();
        }

        public override Task OnDisconnected()
        {
            TryCancelCurrentReadingForConnection();
            return base.OnDisconnected();
        }

        public void TryCancelCurrentReadingForConnection()
        {
            CancellationTokenSource cts;
            if (cancellationTokens.TryRemove(Context.ConnectionId, out cts))
            {
                cts.Cancel();
            }
        }
    }
}