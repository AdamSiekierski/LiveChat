using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LiveChat.Data;
using LiveChat.Models;

namespace LiveChat.Pages
{
    public class RoomModel : PageModel
    {
        private readonly LiveChat.Data.ApplicationDbContext _context;

        public RoomModel(LiveChat.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Room Room { get; set; } = default!; 

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
            else 
            {
                Room = room;
            }
            return Page();
        }
    }
}
