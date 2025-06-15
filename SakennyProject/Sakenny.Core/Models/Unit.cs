using Sakenny.Core.Helpers;

namespace Sakenny.Core.Models
{
    public class Unit : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string? Street { get; set; }
        public ushort Floor { get; set; }
        public ushort? RoomsCount { get; set; }
        public string? FrontPicture { get; set; }
        public ushort BathRoomCount { get; set; }
        public ushort? BedRoomCount { get; set; }
        public decimal? Price { get; set; }
        public decimal UnitArea { get; set; }
        public double Rate { get; set; }
        public int CountRated { get; set; } = 0;
        public bool IsRented { get; set; }
        public bool IsFurnished { get; set; }
        public Location Location { get; set; }
        public RentalFrequency? RentalFrequency { get; set; }
        public NearbyServices NearbyServices { get; set; }
        public UnitServices UnitServices { get; set; }
        public GenderType GenderType { get; set; }
        public UnitType UnitType { get; set; }
        public ICollection<UnitPicutre> PicutresUrl { get; set; } = new List<UnitPicutre>();
        public ICollection<Rating> Ratings { get; set; }=new HashSet<Rating>();
        public string UserId { get; set; }
        public User User { get; set; }
    }
}