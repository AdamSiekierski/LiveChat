using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

			await Clients.Group(roomId.ToString()).SendAsync("MessageSent", message.Author.Email, message.Content, message.ID);
		}

		public async Task DeleteMessage(int messageId, int roomId,
            UserManager<IdentityUser> _userManager,
            ApplicationDbContext _dbContext) {
			var user = await _userManager.FindByIdAsync(Context.UserIdentifier);
			var room = _dbContext.Rooms.Where(r => r.ID == roomId).Include(r => r.Admin).First();
			var message = _dbContext.Messages.Where(m => m.ID == messageId).Include(m => m.Author).First();

			var userId = user?.Id;

            var isAdmin = room.Admin.Id == userId;
			var isAuthor = message.Author.Id == userId;

            if (message.Room.ID == room.ID && (isAdmin || isAuthor))
            {
                _dbContext.Messages.Remove(message);
				_dbContext.SaveChanges();

				await Clients.Group(roomId.ToString()).SendAsync("MessageRemoved", messageId.ToString());
			}
        }
	}
}
