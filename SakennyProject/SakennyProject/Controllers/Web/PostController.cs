using Microsoft.AspNetCore.Mvc;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository;
using Sakenny.Repository.Data;
using SakennyProject.DTO;
using SakennyProject.Erorrs;

namespace SakennyProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostRepo repo;
        private readonly SakennyDbContext sakennyDb;

        public  PostController(PostRepo repo,SakennyDbContext sakennyDb)
        {
            this.repo = repo;
            this.sakennyDb = sakennyDb;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<PostAdminDto>>> Get([FromQuery]PostSpecParam param)
        {
            var posts = await repo.Get(param);
            return Ok(posts);
        }
        [HttpPut("Delete")]
        public async Task<ActionResult<bool>> Delete(int Id)
        {
            var post = await sakennyDb.Posts.FindAsync(Id);
            if (post == null)
                return BadRequest( new ApiResponse(400,"There is no post has this id"));
            await repo.Delete(post);
            return Ok();
        }
        [HttpPut("Restore")]
        public async Task<ActionResult<bool>> Restore(int Id)
        {
            var post = await sakennyDb.Posts.FindAsync(Id);
            if (post == null)
                return BadRequest(new ApiResponse(400, "There is no post has this id"));
            await repo.Restore(post);
            return Ok();
        }

    }
}
