using System;
using Microsoft.AspNetCore.Identity;

namespace LiveChat.Models
{
    public class Message
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public Room Room { get; set; }
        public IdentityUser Author { get; set; }
    }
}

