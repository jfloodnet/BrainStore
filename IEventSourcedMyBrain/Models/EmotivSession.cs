using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEventSourcedMyBrain.Models
{
    public class EmotivSession
    {
        public string StreamId { get; set; }
        public string UserActivity { get; set; }
        public string SessionStarted { get; set; }
        public string SessionEnded { get; set; }
        public string DisplayName
        {
            get
            {
                return SessionStarted + " - " + SessionEnded;
            }
        }
    }
}