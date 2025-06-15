using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sakenny.Core.Models;
using Sakenny.Core.Repositories;
using Sakenny.Core.Specification;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository;
using Sakenny.Repository.Data;
using Sakenny.Services;
using SakennyProject.DTO.Chat;
using SakennyProject.Erorrs;
using System.Security.Claims;

namespace SakennyProject.Controllers.Mobile
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly ChatRepo chatRepo;
        private readonly SakennyDbContext sakennyDb;
        private readonly ChatService chatService;

        public ChatController(ChatRepo chatRepo,
            SakennyDbContext sakennyDb,
            ChatService chatService
            ) {
            this.chatRepo =chatRepo ;
            this.sakennyDb = sakennyDb;
            this.chatService = chatService;
        }
        [HttpGet("Get")]
        public async Task<ActionResult<IReadOnlyList<ChatListDto>>> Get([FromQuery]BaseSpecParam param)
        {
            var UserId = HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
          if (string.IsNullOrEmpty(UserId))
                return BadRequest(new ApiResponse(400,"No Id in Token"));
            var res= await chatRepo.GetAll(param, UserId);
            return Ok(res);
        }
        [HttpGet("GetContentById")]
        public async Task<ActionResult<IReadOnlyList<Messages>>> GetById([FromQuery]ChatSpecParam param)
        {
           var res=await chatRepo.GetContent(param);
            return Ok(res);
        }
        [HttpPost("SendMsg")]
        public async Task<ActionResult<bool>> Send(MessageDto message)
        {
            message.SendedAt = DateTime.Now;       
            var res=await chatService.SendPrivate(message);
            return Ok(res);
        }
        [HttpPut("MarkRead")]
        public async Task<ActionResult<bool>> MarkRead(int MsgId)
        {
            
            var res= await chatService.MarkRead( MsgId);
            return Ok(res);
        }
    }
}
