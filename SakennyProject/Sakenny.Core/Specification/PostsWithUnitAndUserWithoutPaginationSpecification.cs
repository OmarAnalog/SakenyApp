using Sakenny.Core.Models;

namespace Sakenny.Core.Specification
{
    public class PostsWithUnitAndUserWithoutPaginationSpecification:BaseSpecification<Post>
    {
        public PostsWithUnitAndUserWithoutPaginationSpecification(PostSpecParams specParams) : base(p => !p.ISDeleted)
        {
        }
    }
}
