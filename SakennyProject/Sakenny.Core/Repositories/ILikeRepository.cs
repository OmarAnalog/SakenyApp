using Sakenny.Core.Models;

namespace Sakenny.Core.Repositories
{
    public interface ILikeRepository
    {
        Task<bool> AddLikeAsync(Like like);
        Task<bool> RemoveLikeAsync(int postId, string userId);
        Task<Like> PostLikedByUserAsync(int postId, string userId);
        Task<IReadOnlyList<Like>> GetLikesByPostIdAsync(int postId);
        Task<HashSet<int>> GetLikesByUserIdAsync(string userId);
        Task<int> GetLikesCountByPostIdAsync(int postId);
        Task<int> GetLikesCountByUserIdAsync(string userId);
    }
}
