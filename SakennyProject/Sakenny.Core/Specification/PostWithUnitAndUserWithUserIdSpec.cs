using Sakenny.Core.Models;

namespace Sakenny.Core.Specification
{
    public class PostWithUnitAndUserWithUserIdSpec:BaseSpecification<Post>
    {
        public PostWithUnitAndUserWithUserIdSpec(PostSpecParams postSpec):base(p => !p.ISDeleted && p.UserId == postSpec.UserId)
        {
            Includes.Add(p=>p.User);
            Includes.Add(p => p.Unit);
            Includes.Add(P => P.Unit.PicutresUrl);
            ApplyPagination(postSpec.PageSize * (postSpec.PageIndex - 1), postSpec.PageSize);
        }
    }
}
