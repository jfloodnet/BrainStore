using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

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
    }
}