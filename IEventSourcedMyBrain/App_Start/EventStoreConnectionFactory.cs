using System.Net;
using EventStore.ClientAPI;

namespace IEventSourcedMyBrain
{
    public class EventStoreConnectionFactory
    {
        public static EventStoreConnection Create()
        {
            var connection = EventStoreConnection.Create();
            connection.Connect(new IPEndPoint(IPAddress.Parse(Config.Host), 1113));
            return connection;
        }
    }
}