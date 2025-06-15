using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;
using Sakenny.Repository.Data;
using SakennyProject.DTO.Chat;
using SakennyProject.Hubs;
using System;
using System.Security.Claims;

namespace Sakenny.Services
{
    public class ChatService
    {
        private readonly IHubContext<ChatHub> hub;
        private readonly SakennyDbContext sakennyDb;
        private readonly NotificationService notificationService;

        public ChatService(IHubContext<ChatHub> hub,
            SakennyDbContext sakennyDb,
            NotificationService notificationService)
        {
            this.hub = hub;
            this.sakennyDb = sakennyDb;
            this.notificationService = notificationService;
        }
        public async Task<bool> MarkRead(int msgId)
        {

            var msg = await sakennyDb.Messages.FindAsync(msgId);
            if (msg == null) return false;
            bool seen=false;
            var chat = await sakennyDb.Chats.FirstOrDefaultAsync(u => u.Id == msg.ChatId);
            if(chat == null) return false;
            if (msg.Read == false)
            {
                if (chat.Count > 0)
                    chat.Count--;
                msg.Read = true;
                seen = true;
                sakennyDb.Messages.Update(msg);
                sakennyDb.Chats.Update(chat);
                sakennyDb.SaveChanges();
            }
            if (seen)
            {
                var connection= await sakennyDb.Connections
                    .Where(u=>u.UserId==msg.SenderId && u.ConnectionType==Core.Helpers.ConnectionType.Chat)
                    .Select(u=>u.Connection)
                    .ToListAsync();
                foreach (var c in connection)
                    hub.Clients.Client(c).SendAsync("Seen",new { msgId,msg.SenderId,msg.ReceiverId});
            }
            return true;
        }
        public async Task<bool> SendPrivate(MessageDto Model)
        {
            if (!Model.ChatId.HasValue)
            {
                if(!string.IsNullOrEmpty(Model.From) &&!string.IsNullOrEmpty(Model.From))
                Model.ChatId = await CreateChat(Model.From, Model.To);
                else return false;
            }
            var msg = new Messages()
            {
                ChatId = Model.ChatId ?? 0,
                Content = Model.Content,
                Date = Model.SendedAt,
                SenderId = Model.From,
                ReceiverId = Model.To,
                Read = false
            };
            Chat chat = await sakennyDb.Chats.FindAsync(msg.ChatId);
            chat.Last = Model.SendedAt;
            chat.LastMsg = Model.Content;
            if(chat.LastId!=msg.SenderId)
                chat.Count = 1;
            else chat.Count = 0;
                chat.LastId = msg.SenderId;
          

            await sakennyDb.Messages.AddAsync(msg);
            sakennyDb.Chats.Update(chat);
            sakennyDb.SaveChanges();
            var ToConnections = await sakennyDb.Connections
                .Where(u => u.UserId == Model.To &&
            u.ConnectionType == Sakenny.Core.Helpers.ConnectionType.Chat)
                .ToListAsync();
            var MyConnection= await sakennyDb.Connections
                .Where(u => u.UserId == Model.From &&
            u.ConnectionType == Sakenny.Core.Helpers.ConnectionType.Chat)
                .ToListAsync();
            foreach (var connection in MyConnection)
                hub.Clients.Client(connection.Connection).SendAsync("NewMessage", msg);
            if (ToConnections.Any())
            {
                foreach (var connection in ToConnections)
                    hub.Clients.Client(connection.Connection).SendAsync("NewMessage", msg);
            }else
            {
               var res= await notificationService.SendMsgNotificatoinAsync(msg);
                return res;
            }
                return true;
        }
        private async Task<int> CreateChat(string ID1, string ID2)
        {
            var chat = await sakennyDb.Chats.FirstOrDefaultAsync(u =>
            (u.SUserId==ID2 &&u.FUserId==ID1)
            ||
            (u.SUserId == ID1 && u.FUserId == ID2)
            );
            if (chat == null)
            {
                chat = new Chat()
                {
                    FUserId = ID1,
                    SUserId = ID2,
                };
                sakennyDb.Chats.Add(chat);
                await sakennyDb.SaveChangesAsync();
            }
            return chat.Id;
        }
    }
}

