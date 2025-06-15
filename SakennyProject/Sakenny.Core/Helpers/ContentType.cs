using System.Runtime.Serialization;

namespace Sakenny.Core.Helpers
{
    public enum ContentType
    {
        [EnumMember(Value = "Comment")]
        Comment,
        [EnumMember(Value = "Post")]
        Post
    }
}
