using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;
using Sakenny.Core.Repositories;
using Sakenny.Core.Specification;
using Sakenny.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakenny.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly SakennyDbContext dbContext;

        public GenericRepository(SakennyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #region WithoutSpecification
        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await dbContext.Set<T>().ToListAsync(); // dbcontext.set<T>
        public async Task<T> GetByIdAsync(int id)
        => await dbContext.Set<T>().FindAsync(id);
        #endregion
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        { 
            var gg = await ApplySpec(spec).ToListAsync(); 
            return gg;
        }
        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        => await ApplySpec(spec).FirstOrDefaultAsync();
        public async Task<IReadOnlyList<T>> GetListWithIdWithSpec(ISpecification<T> spec)
           => await ApplySpec(spec).ToListAsync();

        public IQueryable<T> ApplySpec(ISpecification<T> spec)
            => SpecificationBuilder<T>.GetQuery(dbContext.Set<T>(), spec);

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        => await ApplySpec(spec).CountAsync();

        public async Task<T> Add(T entity)
        {
            try
            {
                var newEntity = await dbContext.Set<T>().AddAsync(entity);
                var rows = await dbContext.SaveChangesAsync();
                return entity;
            }catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<int> Update(T entity)
        {
            var updateEntity = dbContext.Set<T>().Update(entity);
            var rows = await dbContext.SaveChangesAsync();
            return rows;
        }

        public async Task<int> Delete(T entity)
        {
            try
            {
                var deleteEntity = dbContext.Set<T>().Remove(entity);
                var rows = await dbContext.SaveChangesAsync();
                return rows;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
