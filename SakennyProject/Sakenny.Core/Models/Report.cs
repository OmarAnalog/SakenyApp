using Sakenny.Core.Helpers;

namespace Sakenny.Core.Models
{
   public class Report :BaseEntity
    {
        public string FromId { get; set; }
        public User From { get; set; }
        public string ToId { get; set; }
        public User To { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public ContentType ContentType { get; set; }=ContentType.Post;
        public Status Status { get; set; } = Status.Pending;
        public ReportTypes ReportTypes { get; set; } = ReportTypes.Other;
        public Helpers.Action Action { get; set; } = Helpers.Action.None;
        public string Description { get; set; }
        public int ContentId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
    }
}
