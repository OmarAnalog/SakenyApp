using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Sakenny.Core.Models;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository;
using SakennyProject.Erorrs;
using System.Security.Claims;

namespace SakennyProject.Controllers.Mobile
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTokenController : ControllerBase
    {
        private readonly UsersRepo usersRepo;
        private readonly UserManager<User> userManager;

        public DeviceTokenController(UsersRepo usersRepo,UserManager<User> userManager)
        {
            this.usersRepo = usersRepo;
            this.userManager = userManager;
        }
        [HttpPost("Add")]

        public async Task<ActionResult<bool>> AddDeviceToken(DeviceTokenSpecParam param)
        {
            var user = await userManager.FindByIdAsync(param.UserId);
            if (user == null) {
                return BadRequest(new ApiResponse(400, "There is no Id"));
            }
            var res= await usersRepo.AddDeviceToken(param);
            return Ok(true);
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult<bool>> Delete([FromQuery]DeviceTokenSpecParam param)
        {

            var user =await  userManager.FindByIdAsync(param.UserId);
            if (user == null) {
                return BadRequest(new ApiResponse(400, "There is no Id"));
            }
            var res=await usersRepo.RemoveDeviceToken(param );
            return Ok(res);
        }
      
    
    }
}
