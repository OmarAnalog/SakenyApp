using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;
using Sakenny.Repository.Data;
using System.Security.Claims;
using SakennyProject.DTO.Chat;

namespace SakennyProject.Hubs
{

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly SakennyDbContext sakennyDb;
        public ChatHub(SakennyDbContext sakennyDb)
        {
            this.sakennyDb = sakennyDb;
        }


        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                sakennyDb.Connections.Add(new Sakenny.Core.Models.Connections()
                {
                    ConnectionType = Sakenny.Core.Helpers.ConnectionType.Chat,
                    Connection = Context.ConnectionId,
                    UserId = userId,
                });

                await sakennyDb.SaveChangesAsync();
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = await sakennyDb.Connections
                .FirstOrDefaultAsync(c => c.Connection == Context.ConnectionId);

            if (connection != null)
            {
                sakennyDb.Connections.Remove(connection);
                await sakennyDb.SaveChangesAsync();
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
      
}
