namespace Sakenny.Core.Models
{
    public class Post : BaseEntity 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool ISDeleted { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Like> Likes { get; set; } = new HashSet<Like>();
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public ICollection<PostFavouriteList> PostFavouriteLists { get; set; } = new HashSet<PostFavouriteList>();

    }
}
