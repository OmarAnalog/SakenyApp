﻿namespace Sakenny.Core.Models
{
    public class Like:BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
