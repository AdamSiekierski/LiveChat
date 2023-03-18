using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LiveChat.Data;
using LiveChat.Models;

namespace LiveChat.Pages
{
    public class EditRoomModel : PageModel
    {
        private readonly LiveChat.Data.ApplicationDbContext _context;

        public EditRoomModel(LiveChat.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room Room { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room =  await _context.Rooms.FirstOrDefaultAsync(m => m.ID == id);
            if (room == null)
            {
                return NotFound();
            }
            Room = room;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(Room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(Room.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Room", new { id = Room.ID });
        }

        private bool RoomExists(int id)
        {
          return (_context.Rooms?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
