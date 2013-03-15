using Emotiv;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Concurrent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ConsciousnessStream.Extensions;

namespace ConsciousnessStream
{
    class Program
    {
        static void Main(string[] args)
        {             
            using (var connection = EventStoreConnection.Create())
            {
                connection.Connect(new IPEndPoint(IPAddress.Parse("54.252.92.203"), 1113));

                var sessionId = Guid.NewGuid();
                var streamName = StreamName.Create(sessionId, "EmoSession");

                var activity = UserActivity.Create("Emulator");
                
                IEventStore eventStore = new EventStoreWrapper(connection,() =>  WriteMetaData(activity));                

                connection.CreateStream(streamName, sessionId, true, WriteMetaData(activity).ToByteArray());

                MindReader reader = new MindReader(eventStore, streamName, EmoEngine.Instance);

                reader.StartReading();                             
            }
        }

        private static Dictionary<string, object> WriteMetaData(UserActivity activity)
        {
            var eventHeaders = new Dictionary<string, object>()
            {
                {"UserActivity", activity.ToString()},
                {"TimeStamp", DateTime.Now.ToString()}
            };
            return eventHeaders;
        }  
    }
}
