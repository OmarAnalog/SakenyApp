using Sakenny.Core.Models;

namespace Sakenny.Core.Helpers
{
    public class MyPostsDto
    {
        public ICollection<Post> Posts { get; set; }= new List<Post>();
        public int Count { get; set; }
    }
}
