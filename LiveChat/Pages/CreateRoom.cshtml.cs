using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LiveChat.Data;
using LiveChat.Models;
using Microsoft.AspNetCore.Identity;

namespace LiveChat.Pages
{
    public class CreateRoomModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateRoomModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            Console.WriteLine(user);
            Console.WriteLine(Room);
            Console.WriteLine(ModelState.IsValid);

            if (_context.Rooms == null || Room == null)
            {
                return Page();
            }

            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
