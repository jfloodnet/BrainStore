using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsciousnessStream
{
    public struct UserActivity
    {
        readonly string _value;

        UserActivity(string activity)
        {
            _value = activity;
        }

        public static implicit operator string(UserActivity name)
        {
            return name._value;
        }

        public static UserActivity Create(string activity)
        {
            return new UserActivity(activity);
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
