using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class UserAdded : EmoEvent
    {
        public UserAdded(uint userId)
            : base(userId)
        {

        }
    }
}
