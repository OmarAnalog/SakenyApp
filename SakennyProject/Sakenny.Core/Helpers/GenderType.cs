using System.Runtime.Serialization;

namespace Sakenny.Core.Helpers
{
    public enum GenderType
    {
        [EnumMember(Value = "Male")]
        Male,
        [EnumMember(Value = "Female")]
        Female,
        [EnumMember(Value = "Any")]
        Any
    }
}
