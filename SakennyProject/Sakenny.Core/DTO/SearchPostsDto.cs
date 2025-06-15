using Sakenny.Core.Specification;

namespace Sakenny.Core.DTO
{
    public class SearchPostsDto : PostSpecParams
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
