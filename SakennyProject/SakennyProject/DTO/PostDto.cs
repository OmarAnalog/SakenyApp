using Sakenny.Core.Helpers;
using Sakenny.Core;
using Sakenny.Core.Models;

namespace SakennyProject.DTO
{
    public class PostDto
    {
        public int PostId { get; set; }
        public double UserRate { get; set; }
        public double UserCountRate { get; set; }
        public string Title { get; set; }
        public string Owner { get; set; }
        public string OwnerId { get; set; }
        public string OwnerPicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsFavourite { get; set; }
        public double Area { get; set; }
        #region unit
        public UnitDto Unit { get; set; }
        #endregion
    }
}
