using Microsoft.Extensions.Primitives;

namespace Sakenny.Core.Models
{
    public class Messages: BaseEntity
    {
        public string Content { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public int ChatId { get; set; }
        public bool Read {  get; set; }=false;
    }
}
