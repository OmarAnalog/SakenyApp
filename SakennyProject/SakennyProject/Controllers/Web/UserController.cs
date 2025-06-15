using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sakenny.Core.DTO;
using Sakenny.Repository;
using Microsoft.AspNetCore.Identity;
using SakennyProject.DTO;
using Sakenny.Core.Models;
using SakennyProject.Erorrs;
using Sakenny.Core.Specification.SpecParam;
namespace SakennyProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiController]
    //[Authorize( Roles ="Admin")]
    public class UserController : ControllerBase
    {
        private readonly UsersRepo users;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public UserController(UsersRepo Users,UserManager<User> userManager,IConfiguration configuration)
        {
            users = Users;
            this.userManager = userManager;
            this.configuration = configuration;
        }
        [HttpGet("GetUsers")]
        public async Task<ActionResult<IReadOnlyList<UserToReturnAdminDto>>> Get([FromQuery]UserSpecParam param)
        {
            var user= await users.Get(param);
            IReadOnlyList<UserToReturnAdminDto> ret= new List<UserToReturnAdminDto>();
            if (user!=null) 
                ret = user;
             return Ok(ret);
        }
        [HttpPost("AddRole")]
        public async Task<ActionResult<bool>> AddtoAdmins(NewAdminDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null) return BadRequest(new ApiResponse(400, "No such User"));
            var result = await userManager.AddToRoleAsync(user, configuration["Roles:Admin"]);
            return Ok(result.Succeeded);
        }
        [HttpPut("DeleteUser")]
        public async Task<ActionResult<bool>> DeleteUser(string Id)
        {
            var user=await userManager.FindByIdAsync(Id);
            if (user is null) return BadRequest(new ApiResponse(400, "No such User"));
            await users.Delete(user);
            return Ok();
        }
        [HttpPut("RestoreUser")]
        public async Task<ActionResult<bool>>RestoreUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user is null) return BadRequest(new ApiResponse(400, "No such User"));
           await  users.Restore(user);
            return Ok();
        }



    }
}
