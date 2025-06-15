namespace Sakenny.Core.Models
{
    public class PostFavouriteList
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int FavouriteListId { get; set; }
        public FavouriteList FavouriteList { get; set; }
    }
}
