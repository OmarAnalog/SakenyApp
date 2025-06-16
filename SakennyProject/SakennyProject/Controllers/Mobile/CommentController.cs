using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sakenny.Core.Helpers;
using Sakenny.Core.Models;
using Sakenny.Core.Repositories;
using Sakenny.Core.Specification;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository;
using Sakenny.Services;
using SakennyProject.DTO;
using SakennyProject.Helper;
using System.Security.Claims;

namespace SakennyProject.Controllers.Mobile
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IGenericRepository<Comment> genericRepository;
        private readonly IMapper mapper;
        private readonly IGenericRepository<Post> postRepository;
        private readonly NotificationService notificationService;
        private readonly UserManager<User> userManager;

        public CommentController(IGenericRepository<Comment> genericRepository,
            IMapper mapper,
            IGenericRepository<Post> postRepository
            ,NotificationService notificationService
            ,UserManager<User> userManager)
        {
            this.genericRepository = genericRepository;
            this.mapper = mapper;
            this.postRepository = postRepository;
            this.notificationService = notificationService;
            this.userManager = userManager;
        }
        [HttpPost("add-comment")]
        public async Task<ActionResult> AddComment(CommentDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return BadRequest("Invalid UserId");
            model.UserId = userId;
            var user = await userManager.FindByIdAsync(userId);
            var comment =mapper.Map<Comment>(model);
            var result = await genericRepository.Add(comment);
            if (result is null)
            {
                return BadRequest("Failed to add comment");
            }
            var post = await postRepository.GetByIdAsync(model.PostId);
            post.CommentsCount++;
            await postRepository.Update(post);
            NotificationToReturnDto notification =
                new()
                {
                ContentId = post.Id
                ,Name = user.FirstName + " " + user.LastName
                ,notificationType = NotificationType.Comment
                ,To = post.UserId
                ,Picture = user.Picture
                ,UserId = userId
                };
            try
            {
                await notificationService.SendNotifcation(notification);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
            }
            return Ok("Comment added successfully");
        }
        [HttpDelete("delete-comment/{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comment = await genericRepository.GetByIdAsync(id);
            if (comment is null)
            {
                return NotFound("Comment not found");
            }
            var post = await postRepository.GetByIdAsync(comment.PostId);
            if (comment.UserId != userId)
            {
                return Unauthorized("You are not authorized to delete this comment");
            }
            if (await genericRepository.Delete(comment)==0)
            {
                return BadRequest("Failed to delete comment");
            }
            post.CommentsCount--;
            await postRepository.Update(post);
            return Ok("Comment deleted successfully");
        }
        [HttpGet("allComments")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllComments([FromQuery]CommentSpectParams param)
        {
            var specComments = new CommentWithUsersWithPagination(param);
            var comments=await genericRepository.GetListWithIdWithSpec(specComments);
            var specCount= new CommentWithUsers(param);
            var count = await genericRepository.GetListWithIdWithSpec(specCount);
            var result = mapper.Map<IReadOnlyList<ReturnedComment>>(comments);
            var Returned = new Pagination<ReturnedComment>(param.PageSize, param.PageIndex, count.Count, result);
            return Ok(Returned);
        }
    }
}
