using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsciousnessStream.Events
{
    public class EmoEvent
    {
        public uint UserId;

        public EmoEvent(uint userId)
        {
            this.UserId = userId;
        }
    }
}
