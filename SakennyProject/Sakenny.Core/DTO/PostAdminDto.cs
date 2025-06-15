using Sakenny.Core.Models;

namespace SakennyProject.DTO
{
    public class PostAdminDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public double Rate { get; set; }
        public ushort RoomsCount { get; set; }
        public ushort BathRoomCount { get; set; }
        public string Address { get; set; }
        public decimal UnitArea { get; set; }
        public ICollection<UnitPicutre> PicutresUrl { get; set; } = new List<UnitPicutre>();
        public decimal? RentPrice { get; set; }
        public decimal? Price { get; set; }
    }
}
