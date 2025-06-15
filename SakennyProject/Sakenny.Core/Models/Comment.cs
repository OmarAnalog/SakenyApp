
namespace Sakenny.Core.Models
{
    public class Comment:BaseEntity
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }
        public User User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public ICollection<Report> reports { get; set; } = new HashSet<Report>();
    }
}