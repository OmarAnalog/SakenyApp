namespace Sakenny.Core.Models
{
    public class Chat : BaseEntity
    {
        public string FUserId { get; set; }
        public User FUser { get; set; }
        public string SUserId { get; set; }
        public User SUser { get; set; }
        public DateTime Last { get; set; } = DateTime.UtcNow;
        public int Count { get; set; } = 0;
        public string LastMsg { get; set; } = "";
        public string LastId { get; set; } = "";
        public ICollection<Messages> Messages { get; set; } = new HashSet<Messages>();
       

    }
}
