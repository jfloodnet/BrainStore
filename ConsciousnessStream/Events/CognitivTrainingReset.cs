using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    public class CognitivTrainingReset : EmoEvent
    {
        public CognitivTrainingReset(uint userId)
            : base(userId)
        {
        }
    }
}
