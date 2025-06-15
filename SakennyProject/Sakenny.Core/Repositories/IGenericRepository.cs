using Sakenny.Core.Models;
using Sakenny.Core.Specification;

namespace Sakenny.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetListWithIdWithSpec(ISpecification<T> spec);
        Task<T> Add(T entity);
        Task<int> Update(T entity);
        Task<int> GetCountAsync(ISpecification<T> spec);
        Task<int> Delete(T entity);
    }
}
