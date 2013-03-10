using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class ExpressivTrainingStarted : EmoEvent
    {
        public ExpressivTrainingStarted(uint userId)
            : base(userId)
        {

        }
    }
}
