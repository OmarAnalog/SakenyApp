using Sakenny.Core.Models;

namespace Sakenny.Core.Specification
{
    public class UnitWithProperites:BaseSpecification<Unit>
    {
        public UnitWithProperites(int id):base(u=>u.Id==id)
        {
            Includes.Add(u => u.PicutresUrl);
            Includes.Add(u => u.User);
        }
    }
}
