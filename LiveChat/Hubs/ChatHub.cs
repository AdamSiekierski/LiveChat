using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using LiveChat.Models;
using LiveChat.Data;

namespace LiveChat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
	{
		public async Task JoinRoom(int roomId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
		}

		public async Task SendMessage(
			int roomId,
			string content,
			UserManager<IdentityUser> _userManager,
			ApplicationDbContext _dbContext
		)
		{
			var user = await _userManager.FindByIdAsync(Context.UserIdentifier);
			var room = _dbContext.Rooms.Find(roomId);

			var message = new Message
			{
				Author = user,
				Room = room,
				Content = content,
				Created = DateTime.Now
			};

			_dbContext.Messages.Add(message);
			_dbContext.SaveChanges();

			await Clients.Group(roomId.ToString()).SendAsync("MessageSent", message.Author.Email, message.Content);
		}
	}
}
