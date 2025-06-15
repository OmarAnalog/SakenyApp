using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;
using Sakenny.Repository.Data;

namespace Sakenny.Repository
{
    public class FavourityListRepository : GenericRepository<FavouriteList>
    {
        private readonly SakennyDbContext dbContext;

        public FavourityListRepository(SakennyDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<FavouriteList> getListByUserIdAsync(string userId)
        {
            var favouriteList = await dbContext.FavouriteLists.FirstOrDefaultAsync(f=>f.UserId==userId);
            return favouriteList;
        }
    }
}
