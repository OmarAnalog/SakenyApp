using Sakenny.Core.Models;

namespace Sakenny.Core.Specification
{
    public class PostWithUnitAndUserWithUserIdNoPaginationSpec : BaseSpecification<Post>
    {
        public PostWithUnitAndUserWithUserIdNoPaginationSpec(PostSpecParams postSpec) : base(p => !p.ISDeleted&&p.UserId == postSpec.UserId)
        {
        }
    }
}
