namespace Sakenny.Core.Models
{
    public class Rating:BaseEntity
    {
        public string RatingUserId { get; set; }
        public User RatingUser { get; set; }
        public string RatedUserId { get; set; }
        public User RatedUser { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public decimal Rate { get; set; }
    }
}
