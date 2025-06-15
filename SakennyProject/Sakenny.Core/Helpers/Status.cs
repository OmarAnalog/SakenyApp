using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sakenny.Core.Helpers
{
    public enum Status
    {
        [EnumMember(Value = "Solved")]
        Solved,
        [EnumMember(Value = "Pending")]
        Pending
    }
}
