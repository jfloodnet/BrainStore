using System.Configuration;

namespace IEventSourcedMyBrain
{
    public class Config
    {
        public static string Host
        {
            get
            {
                return ConfigurationManager.AppSettings["EventStore.Host"];
            }
        }

        public static int Port
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["EventStore.Port"]);
            }
        }

        public static int HistoricalSessionDispatchInterval
        {
            get { return int.Parse(ConfigurationManager.AppSettings["SignalR.HistoricalSessions.DispatchInterval"]); }
        }
    }
}