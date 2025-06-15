using Sakenny.Core.Models;
using System.Linq.Expressions;

namespace Sakenny.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new();
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagination { get; set; } = false;
        public BaseSpecification()
        {

        }
        public BaseSpecification(Expression<Func<T, bool>>criteriaExpression)
        {
            Criteria = criteriaExpression;
        }
        public void ApplyPagination(int skip,int take)
        {
            Take = take;
            Skip = skip;
            IsPagination = true;
        }
    }
}
