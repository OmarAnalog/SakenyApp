using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sakenny.Core.Helpers
{
    public enum Action
    {
        [EnumMember(Value = "None")]

        None,
        [EnumMember(Value = "DeleteContent")]
        DeleteContent,
        [EnumMember(Value = "DeleteUser")]
        DeleteUser,
        [EnumMember(Value = "NoAction")]
        NoAction,
    }
}
