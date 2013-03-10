using ConsciousnessStream.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class InternalStateChanged : EmoEvent
    {
        public InternalStateChanged(uint userId)
            : base(userId)
        {

        }
    }
}
