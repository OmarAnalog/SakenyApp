using Sakenny.Core.Helpers;
using Sakenny.Core.Models;
using Sakenny.Core;

namespace SakennyProject.DTO
{
    public class UnitDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public ushort Floor { get; set; }
        public ushort? RoomsCount { get; set; }
        public string? FrontPicture { get; set; }
        public ushort BathRoomCount { get; set; }
        public ushort? BedRoomCount { get; set; }
        public decimal? Price { get; set; }
        public decimal UnitArea { get; set; }
        public double Rate { get; set; }
        public double? VistorRate { get; set; }
        public int CountRated { get; set; }
        public bool IsRented { get; set; }
        public bool IsFurnished { get; set; }
        public Location Location { get; set; }
        public string? RentalFrequency { get; set; }
        public NearbyServices NearbyServices { get; set; }
        public UnitServices UnitServices { get; set; }
        public string GenderType { get; set; }
        public string UnitType { get; set; }
        public ICollection<string> PicutresUrl { get; set; } = new List<string>();
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public decimal UserRate { get; set; }
        public int UserCountRated { get; set; }
        public string? UserPicture { get; set; }
        public double UserLongitude { get; set; }
        public double UserLatitude { get; set; }
        public string UserAddress { get; set; }
    }
}
