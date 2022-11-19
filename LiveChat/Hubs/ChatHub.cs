using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;

using LiveChat.Models;
using LiveChat.Data;

namespace LiveChat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
	{
		private UserManager<IdentityUser> _userManager;
		private ApplicationDbContext _dbContext;

		ChatHub(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
		{
			_userManager = userManager;
			_dbContext = dbContext;
		}

		public async Task JoinRoom(int roomId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
		}

		public async Task SendMessageToRoom(int roomId, string content)
		{
			var user = await _userManager.FindByIdAsync(Context.UserIdentifier);
			var room = _dbContext.Rooms.Find(roomId);

			var message = new Message
			{
				Author = user,
				Room = room,
				Content = content,
			};

			_dbContext.Messages.Add(message);
			_dbContext.SaveChanges();

			await Clients.Group(roomId.ToString()).SendAsync("MessageSent", message);
        }
	}
}
