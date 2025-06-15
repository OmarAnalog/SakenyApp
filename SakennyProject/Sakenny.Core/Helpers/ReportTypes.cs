using System.Runtime.Serialization;

namespace Sakenny.Core.Helpers
{
    public enum ReportTypes
    {
        [EnumMember(Value = "Verbal Abuse")]
        VerbalAbuse,
        [EnumMember(Value = "Misleading Information")]
        MisleadingInformation,
        [EnumMember(Value = "Other")]
        Other
    }
}
