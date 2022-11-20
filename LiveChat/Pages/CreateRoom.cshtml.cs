using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LiveChat.Data;
using LiveChat.Models;
using LiveChat.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace LiveChat.Pages
{
    public class CreateRoomModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public CreateRoomModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, IHubContext<ChatHub> chatHubContext)
        {
            _context = context;
            _userManager = userManager;
            _chatHubContext = chatHubContext;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Room Room { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            Room.Admin = user;
            Room.Created = DateTime.Now;

            if (_context.Rooms == null || Room == null)
            {
                return Page();
            }

            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();

            await _chatHubContext.Clients.All.SendAsync("RoomCreated", Room);

            return RedirectToPage("./Index");
        }
    }
}
