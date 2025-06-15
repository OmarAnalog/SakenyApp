using Sakenny.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakenny.Core.DTO
{
    public class SingleReportDto
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string FId { get; set; }
        public string SName { get; set; }
        public string SId { get; set; }
        public DateTime Time { get; set; }
        public ContentType type { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public Helpers.Action Action { get; set; }
        public int ContentId { get; set; }
        public object Content { get; set; }
        public int? ContentReportCount { get;set; }
    }
}
