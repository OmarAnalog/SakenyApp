namespace SakennyProject.DTO.Chat
{
    public class ChatListDto
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string UserId { get; set; }
        public string LastMsg { get; set; }
        public string LastId { get; set; }
        public string Name { get; set; }
        public string PicUrl { get; set; }
        
        public DateTime Last {  get; set; }

    }
}
