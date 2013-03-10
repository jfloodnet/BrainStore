using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class CognitivTrainingDataErased : EmoEvent
    {
        public CognitivTrainingDataErased(uint userId)
            : base(userId)
        {
        }
    }
}
