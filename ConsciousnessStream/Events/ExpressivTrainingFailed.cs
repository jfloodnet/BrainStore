using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class ExpressivTrainingFailed : EmoEvent
    {
        public ExpressivTrainingFailed(uint userId)
            : base(userId)
        {
        }
    }
}
