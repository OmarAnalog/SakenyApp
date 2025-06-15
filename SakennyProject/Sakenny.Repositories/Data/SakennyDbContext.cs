using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;
using System.Reflection;

namespace Sakenny.Repository.Data
{
    public class SakennyDbContext : IdentityDbContext<User>
    {
        public SakennyDbContext(DbContextOptions<SakennyDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Connections> Connections { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<FavouriteList> FavouriteLists { get; set; }
        public DbSet<PostFavouriteList> PostFavouriteLists { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<DeviceToken> DeviceTokens { get; set; }    
    }
}
