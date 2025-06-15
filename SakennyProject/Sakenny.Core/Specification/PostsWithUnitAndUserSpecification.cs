using Sakenny.Core.Models;

namespace Sakenny.Core.Specification
{
    public class PostsWithUnitAndUserSpecification:BaseSpecification<Post>
    {
        public PostsWithUnitAndUserSpecification(PostSpecParams specParams):base(p=>!p.ISDeleted)
        {
            Includes.Add(P=>P.Unit);
            Includes.Add(P => P.User);
            Includes.Add(P=>P.Unit.PicutresUrl);
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }
    }
}
