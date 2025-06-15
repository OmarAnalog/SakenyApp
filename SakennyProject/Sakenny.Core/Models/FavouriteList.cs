namespace Sakenny.Core.Models
{
    public class FavouriteList:BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<PostFavouriteList> FavouriteListPost { get; set; } = new HashSet<PostFavouriteList>();
    }
}
