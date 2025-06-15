using Sakenny.Core.Models;
using Sakenny.Core.Specification.SpecParam;

namespace Sakenny.Core.Specification
{
    public class CommentWithUsers: BaseSpecification<Comment>
    {
        public CommentWithUsers(CommentSpectParams specParams) : base(p => p.PostId == specParams.PostId)
        {
        }
    }
}
