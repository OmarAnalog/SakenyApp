using Sakenny.Core.Models;
using Sakenny.Core.Specification.SpecParam;

namespace Sakenny.Core.Specification
{
    public class CommentWithUsersWithPagination:BaseSpecification<Comment>
    {
        public CommentWithUsersWithPagination(CommentSpectParams specParams) : base(p=>p.PostId==specParams.PostId)
        {
            Includes.Add(p => p.User);
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
            setAsc();
        }
    }
}
