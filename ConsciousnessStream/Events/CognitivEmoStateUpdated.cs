using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    public class CognitivEmoStateUpdated : EmoEvent
    {
        public string Action;
        public float Power;
        public bool IsActive;

        public CognitivEmoStateUpdated(uint userId, string action, float power, bool isActive) : base(userId)
        {
            this.Action = action;
            this.Power = power;
            this.IsActive = isActive;
        }
    }
}
