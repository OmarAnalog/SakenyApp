using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sakenny.Core.Helpers
{
    public enum ConnectionType
    {
        [EnumMember(Value = "Notification")]
        Notification,
        [EnumMember(Value = "Chat")]
        Chat,
    }
}
