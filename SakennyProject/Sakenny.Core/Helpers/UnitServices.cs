using System.Runtime.Serialization;

namespace Sakenny.Core.Helpers
{
    [Flags]
    public enum UnitServices
    {
        [EnumMember(Value = "Wifi")]
        Wifi=1,
        [EnumMember(Value = "TV")]
        TV=2,
        [EnumMember(Value = "WaterHeater")]
        WaterHeater=4,
        [EnumMember(Value = "AirConditoner")]
        AirConditoner=8,
        [EnumMember(Value = "NaturalGas")]
        NaturalGas = 16,
        [EnumMember(Value = "Elevator")]
        Elevator = 32,
        [EnumMember(Value = "Balcony")]
        Balcony = 64,
        [EnumMember(Value = "LandLine")]
        LandLine = 128,
    }
}
