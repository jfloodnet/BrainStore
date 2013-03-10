using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class CognitivTrainingCompleted : EmoEvent
    {
        public CognitivTrainingCompleted(uint userId)
            : base(userId)
        {
        }
    }
}
