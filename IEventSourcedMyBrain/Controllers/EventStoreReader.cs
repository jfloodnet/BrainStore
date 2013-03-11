using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
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

        public IEnumerable<TEvent> ReadAll<TEvent>(string streamName)
        {
            StreamEventsSlice currentSlice;
            var nextSliceStart = 1;
            do
            {
                currentSlice = this.connection.ReadStreamEventsForward(streamName, nextSliceStart, ReadPageSize, false);
                nextSliceStart = currentSlice.NextEventNumber;

                foreach (var evnt in currentSlice.Events)
                    yield return DeserializeEvent<TEvent>(evnt.OriginalEvent.Data);
            } while (!currentSlice.IsEndOfStream);
        }

        public TEvent DeserializeEvent<TEvent>(byte[] data)
        {
            return JsonConvert.DeserializeObject<TEvent>(data.ReadAsString());
        }
    }
}