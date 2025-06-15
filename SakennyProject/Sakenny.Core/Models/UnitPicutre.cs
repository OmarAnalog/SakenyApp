namespace Sakenny.Core.Models
{
    public class UnitPicutre:BaseEntity
    {
        public string Url { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
