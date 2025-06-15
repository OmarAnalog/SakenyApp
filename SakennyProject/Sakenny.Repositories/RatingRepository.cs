using Microsoft.AspNetCore.Identity;
using Sakenny.Core.Models;
using Sakenny.Repository.Data;
using Sakenny.Repository.Migrations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sakenny.Repository
{
    public class RatingRepository : GenericRepository<Rating>
    {
        private readonly SakennyDbContext dbContext;
        private readonly UserManager<User> userManager;

        public RatingRepository(SakennyDbContext dbContext,UserManager<User> userManager) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public async Task<Rating?> AddRating(Rating rating,Unit unit,User ratedUser)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var addedRate = await dbContext.Set<Rating>().AddAsync(rating);
                if (addedRate == null) return addedRate?.Entity;
                unit.CountRated += 1;
                unit.Rate = (unit.Rate * (unit.CountRated - 1) + (double)rating.Rate)
                    / unit.CountRated;
                var updatedUnit = dbContext.Set<Unit>().Update(unit);
                if (updatedUnit == null) return null;
                ratedUser.CountRated++;
                ratedUser.Rate = (ratedUser.Rate * (ratedUser.CountRated - 1) + rating.Rate)
                    / ratedUser.CountRated;
                var updatedRatedUser = await userManager.UpdateAsync(ratedUser);
                if (updatedRatedUser == null) return null;
                await transaction.CommitAsync();
                return rating;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }
        public async Task<Rating?> UpdateRating(Rating rating,decimal newRate)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                rating.RatedUser.Rate = (rating.RatedUser.Rate * (rating.RatedUser.CountRated) + (newRate - rating.Rate))
                / rating.RatedUser.CountRated;
                rating.Unit.Rate = (rating.Unit.Rate * (rating.Unit.CountRated) + ((double)newRate - (double)rating.Rate))
                    / rating.Unit.CountRated;
                rating.Rate = newRate;
                var updatedRate =  dbContext.Update(rating);
                if (updatedRate == null) return null;
                var updatedUnit = dbContext.Set<Unit>().Update(rating.Unit);
                if (updatedUnit == null) return null;
                var updatedRatedUser = await userManager.UpdateAsync(rating.RatedUser);
                if (updatedRatedUser == null) return null;
                await transaction.CommitAsync();
                return rating;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }
    }
}
