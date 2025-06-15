using Sakenny.Core;
using Sakenny.Core.Helpers;
using Sakenny.Core.Models;

namespace SakennyProject.DTO
{
    public class AddPostDto
    {
        public List<IFormFile> Pictures { get; set; } = new();
        public decimal Area { get; set; }
        public ushort Floor { get; set; }
        public ushort? RoomsCount { get; set; }
        public ushort? BedRoomCount { get; set; }
        public ushort BathRoomCount { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public Location Location { get; set; }
        public bool IsFurnished { get; set; }
        public decimal Price { get; set; }
        public GenderType GenderType { get; set; }
        public RentalFrequency? RentalFrequency { get; set; }
        public NearbyServices NearbyServices { get; set; }
        public UnitType UnitType { get; set; }
        public UnitServices UnitServices { get; set; }
    }
}
