using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    public class CognitivSignatureUpdated : EmoEvent
    {
        public CognitivSignatureUpdated(uint userId)
            : base(userId)
        {
        }
    }
}
