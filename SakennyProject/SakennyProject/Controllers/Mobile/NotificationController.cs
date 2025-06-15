using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository;
using SakennyProject.DTO;
using SakennyProject.Erorrs;
using System.Security.Claims;

namespace SakennyProject.Controllers.Mobile
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly NotifcationRepo repo;

        public NotificationController(NotifcationRepo repo)
        {
            this.repo = repo;
        }
        [HttpGet("Get")]
        public async Task<ActionResult<IReadOnlyList<NotificationToReturnDto>>> Get([FromQuery]BaseSpecParam param)
        {
            var UserId=HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserId == null) {
                return BadRequest(new ApiResponse(400, "Invalid Id"));
            }
            var noti=await repo.GetByUserId(UserId,param);
            return Ok(noti);
        }

    }
}
