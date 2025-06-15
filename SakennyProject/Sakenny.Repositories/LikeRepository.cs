using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;
using Sakenny.Core.Repositories;
using Sakenny.Repository.Data;

namespace Sakenny.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly SakennyDbContext dbContext;

        public LikeRepository(SakennyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task<bool> AddLikeAsync(Like like)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Like>> GetLikesByPostIdAsync(int postId)
        {
            throw new NotImplementedException();
        }

        public async Task<HashSet<int>> GetLikesByUserIdAsync(string userId)
        {
            var posts = dbContext.Likes
                .Where(u => u.UserId == userId).Select(l=>l.PostId).ToHashSet();
            return posts;
        }

        public Task<int> GetLikesCountByPostIdAsync(int postId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetLikesCountByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Like> PostLikedByUserAsync(int postId, string userId)
        {
            var getLike = dbContext.Likes
                .Where(l => l.PostId == postId && l.UserId == userId)
                .FirstOrDefaultAsync();
            return getLike;
        }

        public async Task<bool> RemoveLikeAsync(int postId, string userId)
        {
            var like=PostLikedByUserAsync(postId, userId).Result;
            if (like != null)
            {
                dbContext.Likes.Remove(like);
                return await dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
