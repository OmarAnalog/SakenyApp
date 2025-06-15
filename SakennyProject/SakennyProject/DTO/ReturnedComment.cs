namespace SakennyProject.DTO
{
    public class ReturnedComment
    {
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string UserImage { get; set; }
        public string Content { get; set; }
        public decimal UserRate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
