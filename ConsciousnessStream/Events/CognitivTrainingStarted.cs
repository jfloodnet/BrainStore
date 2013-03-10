using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class CognitivTrainingStarted : EmoEvent
    {
        public CognitivTrainingStarted(uint userId)
            : base(userId)
        {
        }
    }
}
