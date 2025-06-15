using System.Runtime.Serialization;

namespace Sakenny.Core.Helpers
{
    public enum UnitType
    {
        [EnumMember(Value = "RoomRental")]
        RoomRental,
        [EnumMember(Value = "ApartmentRental")]
        ApartmentRental,
        [EnumMember(Value = "HouseRental")]
        HouseRental,
        [EnumMember(Value = "Student's Rental Appartment")]
        StudentsRental,
        [EnumMember(Value = "ApartmentSale")]
        ApartmentSale,
        [EnumMember(Value = "HouseSale")]
        HouseSale
    }
}
