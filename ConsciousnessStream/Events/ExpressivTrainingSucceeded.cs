using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class ExpressivTrainingSucceeded : EmoEvent
    {
        public ExpressivTrainingSucceeded(uint userId)
            : base(userId)
        {

        }
    }
}
