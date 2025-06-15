using Microsoft.EntityFrameworkCore;
using Sakenny.Core.DTO;
using Sakenny.Core.Models;
using Sakenny.Core.Specification;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository.Data;
using SakennyProject.DTO;

namespace Sakenny.Repository
{
    public class PostRepo
    {
        private readonly SakennyDbContext sakennyDb;

        public PostRepo(SakennyDbContext sakennyDb)
        {
            this.sakennyDb = sakennyDb;
        }
        public async Task<IReadOnlyList<PostAdminDto>> Get(PostSpecParam param)
        {

            var posts = await sakennyDb.Posts
                .Where(p => p.ISDeleted == param.Hidden)
                .OrderByDescending(u => u.Id)
                .Skip((param.PageIndex - 1) * param.PageIndex)
                .Take(param.PageSize)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .Include(p => p.Unit).Select(p => new PostAdminDto
                {
                    Id = p.Id,
                    Likes = p.LikesCount,
                    Comments = p.CommentsCount,
                    Rate = p.Unit.Rate,
                    BathRoomCount = p.Unit.BathRoomCount,
                    PicutresUrl = p.Unit.PicutresUrl,
                    RoomsCount = p.Unit.RoomsCount??0,
                    Title = p.Unit.Title,
                    UnitArea = p.Unit.UnitArea,
                    Price = p.Unit.Price,
                    Address = p.Unit.Address
                }).ToListAsync();
            return posts;
        }
        public async Task<IEnumerable<Comment>> GetCommentsOfPostById(int postId,int pageSize,int pageIndex)
        {
            var comments = await sakennyDb.Comments
           .Where(c => c.PostId == postId)
           .Include(c => c.User)
           .OrderByDescending(c => c.CreatedAt) // Or any order you want
           .Skip((pageIndex - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();

            return comments;
        }
        public async Task<SearchedPostsReturned> SearchPostsAsync(SearchPostsDto model)
        {
            var query = sakennyDb.Posts
                .Include(p => p.Unit)
                .Include(p => p.User)
                .Where(p => !p.ISDeleted)
                .Where(p => EF.Functions.Like(p.Unit.Title, $"%{model.SearchTerm}%") ||
                EF.Functions.Like(p.Unit.Address, $"%{model.SearchTerm}%")
                || EF.Functions.Like(p.Description, $"%{model.SearchTerm}%"));
            var count = query.Count();
            var posts = await query
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .OrderByDescending(p => p.Id)
                .ToListAsync();
            var searchedPosts = new SearchedPostsReturned() { Count = count, Posts = posts };
            return searchedPosts;
        }
        public async Task<bool> Delete(Post post)
        {

            post.ISDeleted = true;
            sakennyDb.Posts.Update(post);
            sakennyDb.SaveChanges();
            return true;
        }
        public async Task<bool> Restore(Post post)
        {
            post.ISDeleted = false;
            sakennyDb.Posts.Update(post);
            sakennyDb.SaveChanges();    
            return true;
        }
    }
            
}
