using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    public class CognitivAutoSamplingNeutralCompleted : EmoEvent
    {
        public CognitivAutoSamplingNeutralCompleted(uint userId) : base(userId)
        {
        }
    }
}
