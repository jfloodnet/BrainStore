﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class ExpressivTrainingDataErased : EmoEvent
    {
        public ExpressivTrainingDataErased(uint userId)
            : base(userId)
        {
        }
    }
}
