using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Sakenny.Core.Models;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository.Data;
using SakennyProject.DTO.Chat;

namespace Sakenny.Repository
{
    public class ChatRepo
    {
        private readonly SakennyDbContext sakennyDb;

        public ChatRepo(SakennyDbContext sakennyDb) {
            this.sakennyDb = sakennyDb;
        }
        public async Task<IReadOnlyList<ChatListDto>> GetAll(BaseSpecParam param, string ID)
        {
            var res = await sakennyDb.Chats
                .Where(u => u.FUserId == ID || u.SUserId == ID)
                .OrderByDescending(u => u.Last)
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(u => new ChatListDto()
                {
                    Id = u.Id,
                    Last = u.Last,
                    Count = u.Count,
                    LastMsg = u.LastMsg,
                    Name = (u.FUserId == ID ? $"{u.SUser.FirstName} {u.SUser.LastName}" : $"{u.FUser.FirstName} {u.FUser.LastName}"),
                    PicUrl = (u.FUserId == ID ? u.SUser.Picture : u.FUser.Picture),
                    UserId = (u.FUserId == ID ? u.SUserId : u.FUserId),
                    LastId=u.LastId
                }
                ).ToListAsync();
            if(res==null )
                return new List<ChatListDto>();
            return res;
        }
        public async Task<IReadOnlyList<Messages>> GetContent(ChatSpecParam param)
        {
            int ChatId = -1;
            ChatId = param?.ChatId ?? -1;

            if (!string.IsNullOrEmpty(param.Id1) && !string.IsNullOrEmpty(param.Id2))
            {
                var chat = await sakennyDb.Chats.FirstOrDefaultAsync(
                    u =>
                (u.SUserId == param.Id1 && u.FUserId == param.Id2)
                ||
                (u.SUserId == param.Id2 && u.FUserId == param.Id1)
                );
                if (chat == null) return new List<Messages>();
                else ChatId = chat.Id;
            }
            if (ChatId == -1) return new List<Messages>();
            var res = await sakennyDb.Messages
               .Where(u => u.ChatId == ChatId)
               .OrderByDescending(u => u.Id)
               .Skip((param.PageIndex - 1) * param.PageSize)
               .Take(param.PageSize)
               .ToListAsync();
            return res;
        }
    }
}
