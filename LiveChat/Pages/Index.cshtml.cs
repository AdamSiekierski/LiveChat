using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LiveChat.Data;
using LiveChat.Models;

namespace LiveChat.Pages
{
    public class RoomsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RoomsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Room> Room { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Rooms != null)
            {
                Room = await _context.Rooms.Include(room => room.Admin).OrderByDescending(room => room.Created).ToListAsync();
            }
        }
    }
}
