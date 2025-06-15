using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;
using Sakenny.Repository.Data;

namespace Sakenny.Repository
{
    public class PostFavouriteListRepository
    {
        private readonly SakennyDbContext dbContext;

        public PostFavouriteListRepository(SakennyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> Add(int favouriteListId, int postId)
        {
            var PostFavouriteList = new PostFavouriteList() 
            { FavouriteListId = favouriteListId 
             , PostId=postId};
            try
            {
                await dbContext.Set<PostFavouriteList>().AddAsync(PostFavouriteList);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> Remove(int favouriteListId, int postId)
        {
            var postFavouriteList = new PostFavouriteList()
            {
                FavouriteListId = favouriteListId
             ,
                PostId = postId
            };
            try
            {
                dbContext.Set<PostFavouriteList>().Remove(postFavouriteList);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<IReadOnlyList<Post>> GetPostsByListId(int listId)
        {
            var posts = await dbContext.PostFavouriteLists
           .Where(pfl => pfl.FavouriteListId == listId)
           .Include(pfl => pfl.Post)
               .ThenInclude(p => p.User)
           .Include(pfl => pfl.Post)
               .ThenInclude(p => p.Unit)
                .ThenInclude(Unit => Unit.PicutresUrl)
           .Select(pfl => pfl.Post)
           .ToListAsync();

            return posts;
        }
    }
}
