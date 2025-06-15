using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sakenny.Core.DTO;
using Sakenny.Core.Models;
using Sakenny.Core.Repositories;
using Sakenny.Core.Specification;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository;
using Sakenny.Services.ImageService;
using SakennyProject.DTO;
using SakennyProject.Helper;
using System.Linq;
using System.Security.Claims;

namespace SakennyProject.Controllers.Mobile
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IGenericRepository<Post> postRepository;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly ILikeRepository likeRepository;
        private readonly IGenericRepository<Unit> unitRepository;
        private readonly ImageService imageService;
        private readonly IGenericRepository<UnitPicutre> unitPicutreRepository;
        private readonly UsersRepo usersRepo;
        private readonly PostFavouriteListRepository postFavouriteListRepository;
        private readonly FavourityListRepository favourityListRepository;
        private readonly IGenericRepository<Report> reportRepository;
        private readonly ReportRepo reportRepo;
        private readonly PostRepo nonGenericPostRepo;

        public PostController(IGenericRepository<Post> postRepository,
            IMapper mapper,
            UserManager<User> userManager,
            ILikeRepository likeRepository,
            IGenericRepository<Unit> unitRepository,
            ImageService imageService,
            IGenericRepository<UnitPicutre> unitPicutreRepository
            ,UsersRepo usersRepo
            ,PostFavouriteListRepository postFavouriteListRepository
            ,FavourityListRepository favourityListRepository
            ,IGenericRepository<Report> reportRepository
            ,ReportRepo reportRepo
            ,PostRepo nonGenericPostRepo)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.likeRepository = likeRepository;
            this.unitRepository = unitRepository;
            this.imageService = imageService;
            this.unitPicutreRepository = unitPicutreRepository;
            this.usersRepo = usersRepo;
            this.postFavouriteListRepository = postFavouriteListRepository;
            this.favourityListRepository = favourityListRepository;
            this.reportRepository = reportRepository;
            this.reportRepo = reportRepo;
            this.nonGenericPostRepo = nonGenericPostRepo;
        }
        [HttpGet("posts")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<Pagination<PostDto>>>> GetAllPosts([FromQuery] PostSpecParams postSpec)
        {
            var Spec = new PostsWithUnitAndUserSpecification(postSpec);
            var posts = await postRepository.GetAllWithSpecAsync(Spec);
            var countSpec=new PostsWithUnitAndUserWithoutPaginationSpecification(postSpec);
            var Count = await postRepository.GetCountAsync(countSpec);
            var mappedPosts = mapper.Map<IReadOnlyList<PostDto>>(posts);
            mappedPosts = await SetUserInteractions(mappedPosts, postSpec.UserId);
            var Returned = new Pagination<PostDto>(postSpec.PageSize,postSpec.PageIndex,Count,mappedPosts);
            return Ok(Returned);
        }
        [HttpPost("Add")]
        public async Task<ActionResult> AddPost([FromForm] AddPostDto model)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null) return BadRequest("User not found");
            var user=await userManager.FindByEmailAsync(userEmail);
            var unit = MappingModels.MapUnit(model, user.Id);
            var unitAdded = await unitRepository.Add(unit);
            if (unitAdded is null) return BadRequest("Error While Adding Unit");
            string frontPicture = string.Empty;
            foreach (var picture in model.Pictures)
            {
                var result = await imageService.UploadImageAsync(picture);
                var unitPicture = new UnitPicutre()
                {
                    UnitId = unitAdded.Id,
                    Url = result
                };
                if (frontPicture.IsNullOrEmpty())
                    frontPicture = result;
                await unitPicutreRepository.Add(unitPicture);
            }
            if (frontPicture.IsNullOrEmpty())return BadRequest("Error While Posting");
            unitAdded.FrontPicture = frontPicture;
            await unitRepository.Update(unitAdded);
            var post = MappingModels.MapPost(model, unitAdded.Id, user.Id);
            var postAdded = await postRepository.Add(post);
            if (postAdded is null) return BadRequest("Error While Posting");
            return Ok("Your post was added Successfully");
        }
        [HttpGet("PostsByUserId")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<Pagination<PostDto>>>> GetPostsByUserId([FromQuery] PostSpecParams postSpec)
        {
            if (postSpec.UserId is null) return BadRequest("User not found");
            var VistorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var spec = new PostWithUnitAndUserWithUserIdSpec(postSpec);
            var posts = await postRepository.GetListWithIdWithSpec(spec);
            var countSpec = new PostWithUnitAndUserWithUserIdNoPaginationSpec(postSpec);
            var postsWithoutPagination = await postRepository.GetListWithIdWithSpec(countSpec);
            var Count = postsWithoutPagination.Count;
            var mappedPosts = mapper.Map<IReadOnlyList<PostDto>>(posts);
            mappedPosts = await SetUserInteractions(mappedPosts, VistorId);
            var Returned = new Pagination<PostDto>(postSpec.PageSize, postSpec.PageIndex, Count, mappedPosts);
            return Ok(Returned);
        }
        [HttpGet("Filter")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<Pagination<PostDto>>>> GetFilteredPosts(FilterPostsSpec model)
        {
            var spec = new PostWithUnitAndUserWithFiltersSpec(model);
            var posts = await postRepository.GetAllWithSpecAsync(spec);
            var countSpec = new PostWithUnitAndUserWithFiltersNoPaginationSpec(model);
            var countPosts = await postRepository.GetCountAsync(countSpec);
            var mappedPosts = mapper.Map<IReadOnlyList<PostDto>>(posts);
            var visitorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            mappedPosts = await SetUserInteractions(mappedPosts, visitorId);
            var Returned = new Pagination<PostDto>(model.PageSize, model.PageIndex, countPosts, mappedPosts);
            return Ok(Returned);
        }
        [HttpPost("Report")]
        public async Task<ActionResult> ReportPost(ReportPostDto model)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null) return BadRequest("User not found");
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user is null) return BadRequest("User not found");
            var post = await postRepository.GetByIdAsync(model.PostId);
            if (post is null) return NotFound("Post not found");
            if (await reportRepo.ReportBefore(user.Id,post.Id))
                return BadRequest("You have already reported this post");
            var report = new Report()
            {
                FromId = user.Id,
                ToId = post.UserId,
                Description = model.Description,
                ReportTypes = model.TypeOfProblem,
                ContentId=post.Id,
                PostId = post.Id
            };
            await reportRepository.Add(report);
            return Ok("Your report was sent successfully");
        }
        [HttpPut("Edit")]
        public async Task<ActionResult>EditPost(EditPostDto model)
        {
            var editorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var post = await postRepository.GetByIdAsync(model.PostId);
            var ownerId = post.UserId;
            if (editorId is null || editorId != ownerId) return Unauthorized("You can't edit this post");
            if (model.IsDeleted is not null && model.IsDeleted==true)
            {
                if (post is null) return NotFound("Post not found");
                post.ISDeleted = !post.ISDeleted;
                await postRepository.Update(post);
            }
            if (model.IsRented is not null && model.IsRented==true)
            {
                var unit=await unitRepository.GetByIdAsync(model.UnitId);
                if (unit is null) return NotFound("Unit not found");
                unit.IsRented = !unit.IsRented;
                await unitRepository.Update(unit);
            }
            return Ok("Your status for the unit/post has changed");
        }
        [HttpGet("Search")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<Pagination<PostDto>>>> SearchPosts([FromQuery] SearchPostsDto postSpec)
        {
            if (postSpec.SearchTerm.IsNullOrEmpty()) return BadRequest("Search term cannot be empty");
            var VistorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var searchedPosts = await nonGenericPostRepo.SearchPostsAsync(postSpec);
            var mappedPosts = mapper.Map<IReadOnlyList<PostDto>>(searchedPosts.Posts);
            mappedPosts = await SetUserInteractions(mappedPosts, postSpec.UserId);
            var Returned = new Pagination<PostDto>(postSpec.PageSize, postSpec.PageIndex, searchedPosts.Count, mappedPosts);
            return Ok(Returned);
        }

        private async Task<IReadOnlyList<PostDto>> SetUserInteractions(IReadOnlyList<PostDto> posts, string? visitorId)
        {
            if (visitorId is null) return posts;

            var postsLikes = await likeRepository.GetLikesByUserIdAsync(visitorId);
            var favouriteListId = await favourityListRepository.getListByUserIdAsync(visitorId);
            var favouriteposts = await postFavouriteListRepository.GetPostsByListId(favouriteListId.Id);
            var favouritePostIds = favouriteposts.Select(p => p.Id).ToHashSet();

            var ratedPosts = await usersRepo.GetRatedPosts(visitorId);
            var ratedPostsDict = ratedPosts.ToDictionary(p => p.UnitId, p => (double?)p.Rate);

            foreach (var post in posts)
            {
                post.IsLiked = postsLikes.Contains(post.PostId);
                post.IsFavourite = favouritePostIds.Contains(post.PostId);

                ratedPostsDict.TryGetValue(post.Unit.Id, out var rate);
                post.Unit.VistorRate = rate;
            }

            return posts;
        }
    }
}
