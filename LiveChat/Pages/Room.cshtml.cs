using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LiveChat.Data;
using LiveChat.Models;

namespace LiveChat.Pages
{
    public class RoomModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RoomModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Room Room { get; set; } = default!;
        public IList<Message> Messages { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.Include(room => room.Admin).FirstOrDefaultAsync(m => m.ID == id);

            if (room == null)
            {
                return NotFound();
            }

            var messages = await _context.Messages
                                .Where(message => message.Room.ID == room.ID)
                                .Include(message => message.Room)
                                .Include(message => message.Author)
                                .OrderByDescending(message => message.Created)
                                .ToListAsync();

            Room = room;
            Messages = messages;

            return Page();
        }
    }
}
