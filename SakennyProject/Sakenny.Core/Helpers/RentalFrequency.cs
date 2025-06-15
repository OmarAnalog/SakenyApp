using System.Runtime.Serialization;

namespace Sakenny.Core
{
    public enum RentalFrequency
    {
        [EnumMember(Value = "Daily")]
        Daily,
        [EnumMember(Value = "Weekly")]
        Weekly,
        [EnumMember(Value = "Monthly")]
        Monthly,
        [EnumMember(Value = "Yearly")]
        Yearly
    }
}
