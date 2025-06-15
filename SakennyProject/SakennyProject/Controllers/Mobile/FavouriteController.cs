using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sakenny.Core.Models;
using Sakenny.Core.Repositories;
using Sakenny.Core.Specification;
using Sakenny.Repository;
using SakennyProject.DTO;
using SakennyProject.Helper;
using System.Security.Claims;

namespace SakennyProject.Controllers.Mobile
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavouriteController : ControllerBase
    {
        private readonly FavourityListRepository listRepository;
        private readonly PostFavouriteListRepository postFavouriteListRepository;
        private readonly IGenericRepository<Post> postRepository;
        private readonly ILikeRepository likeRepository;
        private readonly IMapper mapper;

        public FavouriteController(FavourityListRepository listRepository,
                                    PostFavouriteListRepository postFavouriteListRepository,
                                    IGenericRepository<Post> postRepository
                                    , ILikeRepository likeRepository
                                    , IMapper mapper)
        {
            this.listRepository = listRepository;
            this.postFavouriteListRepository = postFavouriteListRepository;
            this.postRepository = postRepository;
            this.likeRepository = likeRepository;
            this.mapper = mapper;
        }
        [HttpGet("getfavourites")]
        public async Task<ActionResult> GetFavourites([FromQuery]PostSpecParams param)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // step 1 -> check if user has favourite list
            var favouriteList = await listRepository.getListByUserIdAsync(userId);
            if (favouriteList == null)
            {
                return NotFound("No favourite list found");
            }
            var posts = await postFavouriteListRepository.GetPostsByListId(favouriteList.Id);
            var count = posts.Count;
            posts = posts.Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).ToList();
            var mappedPosts = mapper.Map<IReadOnlyList<PostDto>>(posts);
            if (userId is not null)
            {
                var postsLikes = await likeRepository.GetLikesByUserIdAsync(userId);
                foreach (var post in mappedPosts)
                {
                    post.IsLiked = postsLikes.Contains(post.PostId);
                    // remember to modify isfavourite
                    post.IsFavourite = true;
                }
            }
            var Returned = new Pagination<PostDto>(param.PageSize, param.PageIndex, count, mappedPosts);
            return Ok(Returned);
        }
        [HttpPost("add")]
        public async Task<ActionResult> AddToFavourites(FavouriteDto model)
        {
            // step 1 -> check if user has favourite list
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var favouriteList = await listRepository.getListByUserIdAsync(userId.Value);

            var res = await postFavouriteListRepository.Add(favouriteList.Id, model.PostId);
            if (res)
                return Ok("Post was added to favourite list");
            return Ok("Post is already Favourite");
        }
        [HttpDelete("delete")]
        public async Task<ActionResult> RemoveFromFavourites(FavouriteDto model)
        {
            // step 1 -> check if user has favourite list
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var favouriteList = await listRepository.getListByUserIdAsync(userId.Value);

            var res = await postFavouriteListRepository.Remove(favouriteList.Id, model.PostId);
            if (res)
                return Ok("Post was removed from favourite list");
            return Ok("Post is already removed from favourite");
        }
    }
}
