using Sakenny.Core.Models;

namespace SakennyProject.DTO
{
    public class CommentDto
    {
        public string Content { get; set; }
        public string? UserId { get; set; }
        public int PostId { get; set; }
    }
}
