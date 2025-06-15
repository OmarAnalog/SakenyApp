using Sakenny.Core.Helpers;

namespace Sakenny.Core.DTO
{
    public class ReportAdminDto
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public DateTime Time { get; set; }
        public ContentType type { get; set; }

        public string Description { get; set; }
        public Status Status { get; set; }
        public Helpers.Action Action { get; set; }
        public int ContentId {  get; set; }
    }
}
