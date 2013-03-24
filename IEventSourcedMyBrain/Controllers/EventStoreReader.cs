using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using IEventSourcedMyBrain.Extensions;

namespace IEventSourcedMyBrain.Controllers
{
    public class EventStoreReader
    {
        private const int ReadPageSize = 500;

        readonly EventStoreConnection connection;

        public EventStoreReader(EventStoreConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<ResolvedEvent> ReadAll(string streamName)
        {
            StreamEventsSlice currentSlice;
            var nextSliceStart = 1;
            do
            {
                currentSlice = this.connection.ReadStreamEventsForward(streamName, nextSliceStart, ReadPageSize, false);
                nextSliceStart = currentSlice.NextEventNumber;

                foreach (var evnt in currentSlice.Events)
                    yield return evnt;
            } while (!currentSlice.IsEndOfStream);
        }

        public TEvent DeserializeEvent<TEvent>(byte[] data)
        {
            return JsonConvert.DeserializeObject<TEvent>(data.ReadAsString());
        }
    }

    public static class ResolvedEventExtensions
    {
        public static IEnumerable<T> As<T> (this IEnumerable<ResolvedEvent> stream)
        {
            return stream.Select(evnt => DeserializeEvent<T>(evnt.OriginalEvent.Data));
        }

        public static TEvent DeserializeEvent<TEvent>(byte[] data)
        {
            return JsonConvert.DeserializeObject<TEvent>(data.ReadAsString());
        }
    }
}