using Sakenny.Core.Helpers;

namespace SakennyProject.DTO
{
    public class ReportPostDto
    {
        public string Description { get; set; }
        public ReportTypes TypeOfProblem { get; set; }
        public int PostId { get; set; }
    }
}
