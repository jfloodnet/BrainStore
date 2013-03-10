using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class ExpressivTrainingCompleted : EmoEvent
    {
        public ExpressivTrainingCompleted(uint userId)
            : base(userId)
        {
        }
    }
}
