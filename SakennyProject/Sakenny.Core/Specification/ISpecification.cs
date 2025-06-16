using Sakenny.Core.Models;
using System.Linq.Expressions;

namespace Sakenny.Core.Specification
{
    public interface ISpecification<T> where T:BaseEntity
    {
        public Expression<Func<T,bool>> Criteria { get; set; }
        public List<Expression<Func<T,object>>> Includes { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagination { get; set; }
        public bool IsDesc { get; set; }
    }
}
