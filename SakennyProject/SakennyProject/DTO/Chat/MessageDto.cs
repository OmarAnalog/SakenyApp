namespace SakennyProject.DTO.Chat
{
    public class MessageDto
    {
        public string Content { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int? ChatId { get; set; }
        public DateTime SendedAt { get; set; }= DateTime.UtcNow;
    }

}
