using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LiveChat.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string RoomName { get; set; }

        public IdentityUser Admin { get; set; }
        public ICollection<Message> Messages { get; set; }

        public DateTime Created { get; set; }
    }
}

