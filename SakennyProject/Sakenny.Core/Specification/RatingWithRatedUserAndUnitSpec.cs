using Sakenny.Core.Models;
using Sakenny.Core.Specification.SpecParam;

namespace Sakenny.Core.Specification
{
    public class RatingWithRatedUserAndUnitSpec:BaseSpecification<Rating>
    {
        public RatingWithRatedUserAndUnitSpec(RatingSpec spec):base(x=>x.RatingUserId==spec.RatingId
        && x.UnitId==spec.UnitId)
        {
            Includes.Add(x => x.Unit);
            Includes.Add(x => x.RatedUser);
        }
    }
}
