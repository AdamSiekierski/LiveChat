using System;
using Microsoft.AspNetCore.Identity;

namespace LiveChat.Models
{
    public class Room
    {
        public int ID { get; set; }
        public string RoomName { get; set; }
        public DateTime CreatedAt { get; set; }

        public IdentityUser Admin { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}

