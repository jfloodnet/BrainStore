using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class CognitivTrainingSucceeded : EmoEvent
    {
        public CognitivTrainingSucceeded(uint userId)
            : base(userId)
        {
        }
    }
}
