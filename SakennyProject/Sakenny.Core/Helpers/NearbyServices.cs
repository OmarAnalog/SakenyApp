using System.Runtime.Serialization;

namespace Sakenny.Core.Helpers
{
    [Flags]
    public enum NearbyServices
    {
        [EnumMember(Value ="Bank")]
        Bank=1,
        [EnumMember(Value = "Parking")]
        Parking=2,
        [EnumMember(Value = "Hospital")]
        Hospital=4,
        [EnumMember(Value = "Restaurant")]
        Restaurant=8,
        [EnumMember(Value = "University")]
        University=16,
        [EnumMember(Value = "Gym")]
        Gym=32,
        [EnumMember(Value = "School")]
        School=64
    }
}
