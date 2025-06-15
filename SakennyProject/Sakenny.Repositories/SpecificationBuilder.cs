using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;
using Sakenny.Core.Specification;

namespace Sakenny.Repository
{
    public static class SpecificationBuilder<T> where T: BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T>inputQuery,ISpecification<T> specification)
        {
            try
            {
                var Query = inputQuery;
                Query = Query.OrderByDescending(u => u.Id);
                if (specification.Criteria is not null)
                {
                    Query = Query.Where(specification.Criteria);
                }
                if (specification.IsPagination)
                {
                    Query = Query.Skip(specification.Skip).Take(specification.Take);
                }
                foreach (var spec in specification.Includes)
                {
                    Query = Query.Include(spec);
                }
                return Query;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
