using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class EmoEngineConnected : EmoEvent
    {
        public EmoEngineConnected(uint userId)
            : base(userId)
        {
            
        }

        
    }
}
