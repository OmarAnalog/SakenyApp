using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sakenny.Core.Models;
using Sakenny.Core.Repositories;
using Sakenny.Services;
using SakennyProject.DTO;
using System.Security.Claims;
using Sakenny.Core.Helpers;

namespace SakennyProject.Controllers.Mobile
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly IGenericRepository<Like> _likeRepository;
        private readonly IGenericRepository<Post> postRepository;
        private readonly IMapper mapper;
        private readonly ILikeRepository nonGenericLikeRepo;
        private readonly NotificationService notificationService;
        private readonly UserManager<User> userManager;

        public LikeController(IGenericRepository<Like> likeRepository
            ,IGenericRepository<Post> postRepository,
            IMapper mapper,
            ILikeRepository NonGenericLikeRepo
            ,NotificationService notificationService
            ,UserManager<User> userManager)
        {
            _likeRepository = likeRepository;
            this.postRepository = postRepository;
            this.mapper = mapper;
            nonGenericLikeRepo = NonGenericLikeRepo;
            this.notificationService = notificationService;
            this.userManager = userManager;
        }
        [HttpPost("add-like")]
        public async Task<ActionResult> AddLike(AddLikeDto like)
        {
            var userId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return BadRequest("User was not found");
            var user = await userManager.FindByIdAsync(userId); 
            var mappedLike = new Like() { PostId = like.PostId, UserId = userId };
            var result = await _likeRepository.Add(mappedLike);
            if (result is null)
            {
                return Ok("You've already liked this post");
            }
            var post = await postRepository.GetByIdAsync(like.PostId);
            post.LikesCount++;
            await postRepository.Update(post);
            NotificationToReturnDto model =
                new() { ContentId = post.Id, Name = user.FirstName + " " + user.LastName
                ,notificationType=NotificationType.Like
                ,To=post.UserId,Picture=user.Picture,UserId=userId};
            await notificationService.SendNotifcation(model);
            return Ok("Like added successfully");
        }
        [HttpDelete("remove-like")]
        public async Task<ActionResult> RemoveLike(AddLikeDto like)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await nonGenericLikeRepo.RemoveLikeAsync(like.PostId,userId);
            if (!result)
            {
                return Ok("You don't like this post");
            }
            var post = await postRepository.GetByIdAsync(like.PostId);
            post.LikesCount--;
            await postRepository.Update(post);
            return Ok("Like removed successfully");
        }
    }
}
