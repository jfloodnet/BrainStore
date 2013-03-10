using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class EmoEngineDisconnected : EmoEvent
    {
        public EmoEngineDisconnected(uint userId)
            : base(userId)
        {
        }
    }
}
