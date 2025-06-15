using Sakenny.Core.Models;

namespace Sakenny.Core.DTO
{
    public class SearchedPostsReturned
    {
        public int Count { get; set; }
        public List<Post> Posts { get; set; }
    }
}
