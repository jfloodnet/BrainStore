using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsciousnessStream
{
    public interface IEventStore
    {
        void Store(StreamName streamName, object @event);
    }

    public class EventStoreWrapper : IEventStore
    {
        private const string EventClrTypeHeader = "EventClrTypeName";
        
        private readonly EventStoreConnection eventStoreConnection;
        private readonly Func<Dictionary<string, object>> writeMetaData;

        public EventStoreWrapper(EventStoreConnection eventStoreConnection, Func<Dictionary<string, object>> writeMetaData)
        {
            this.eventStoreConnection = eventStoreConnection;
            this.writeMetaData = writeMetaData;
        }

        public void Store(StreamName streamName, object @event)
        {
            eventStoreConnection.AppendToStream(streamName, ExpectedVersion.Any, JsonEventData.Create(Guid.NewGuid(), @event, this.writeMetaData));
        }

        private static class JsonEventData
        {
            public static EventData Create(Guid eventId, object evnt, Func<Dictionary<string, object>> writeMetaData)
            {
                var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, SerializerSettings));
                var metadata = AddEventClrTypeHeaderAndSerializeMetadata(evnt, writeMetaData);
                var typeName = evnt.GetType().Name;

                return new EventData(eventId, typeName, true, data, metadata);
            }
            private static byte[] AddEventClrTypeHeaderAndSerializeMetadata(object evnt, Func<Dictionary<string, object>> writeMetaData)
            {
                var eventHeaders = new Dictionary<string, object>(writeMetaData())
                {
                    {EventClrTypeHeader, evnt.GetType().AssemblyQualifiedName}
                };

                return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, SerializerSettings));
            }

            private static readonly JsonSerializerSettings SerializerSettings;

            static JsonEventData()
            {
                SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
            }
        }
    }
}
