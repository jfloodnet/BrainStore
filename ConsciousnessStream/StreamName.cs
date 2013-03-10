using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsciousnessStream
{
    public struct StreamName
    {
        readonly string _value;

        StreamName(Guid id, string type)
        {
            _value = string.Format("{0}-{1}", type, id.ToString("N"));
        }

        public static implicit operator string(StreamName name)
        {
            return name._value;
        }

        public static StreamName Create(Guid id, string type)
        {
            return new StreamName(id, type);
        }
    }
}
