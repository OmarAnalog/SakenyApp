using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sakenny.Core.Helpers
{
    public enum NotificationType {

        [EnumMember(Value = "advertisement")]
        advertisement,
        [EnumMember(Value = "Alert")]
        Alert,
        [EnumMember(Value = "Like")]
        Like,
        [EnumMember(Value = "Comment")]
        Comment,
        [EnumMember(Value = "Message")]
        Message
    }
}
