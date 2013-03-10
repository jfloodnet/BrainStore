using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class UserRemoved : EmoEvent
    {
        public UserRemoved(uint userId)
            : base(userId)
        {

        }
    }
}
