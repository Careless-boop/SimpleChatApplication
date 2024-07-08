using Microsoft.AspNetCore.SignalR;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.BLL.Services.Interfaces;
using SimpleChatApplication.DAL.Entities;
using System.Collections.Concurrent;

namespace SimpleChatApplication.BLL.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private static ConcurrentDictionary<string, int> UserConnections = new ConcurrentDictionary<string, int>();
        public ChatHub(IChatService chatService, IUserService userService) 
        {
            _chatService = chatService;
            _userService = userService;
        }

        public override async Task OnConnectedAsync()
        {
            //assume userId passed in headers
            var userId = Context.GetHttpContext().Request.Headers["UserId"];
            if (int.TryParse(userId, out int parsedUserId)) 
            {
                UserConnections[Context.ConnectionId] = parsedUserId;
                var chats = await _chatService.GetChatsByUserId(parsedUserId);
                if (chats.Any())
                {
                    foreach(var chat in chats)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
                    }
                }
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid User Id");
            }
            await Clients.Caller.SendAsync("ReceiveMessage", "You've successfully connected!");
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            UserConnections.TryRemove(Context.ConnectionId, out _);
            
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(int chatId, string message)
        {
            try
            {
                //assume userId passed in headers
                var user = Context.GetHttpContext().Request.Headers["UserId"];
                
                if(int.TryParse(user, out int userId))
                {
                    var newMessage = await _userService.AddMessageToUser(chatId, userId, message);
                    await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", newMessage.UserId, newMessage.Content);
                }
            }
            catch(Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveError", ex.Message);
            }
        }
        public async Task JoinChat(int chatId)
        {
            try
            {
                var user = Context.GetHttpContext().Request.Headers["UserId"];
                if (int.TryParse(user, out int userId))
                {
                    await _chatService.AddUserToChatAsync(chatId, userId);
                    await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
                    await Clients.Group(chatId.ToString()).SendAsync("ReceiveNotification", $"{userId} has joined the chat");
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveError", ex.Message);
            }
        }
        public async Task LeaveChat(int chatId)
        {
            try
            {
                var user = Context.GetHttpContext().Request.Headers["UserId"];
                if (int.TryParse(user, out int userId))
                {
                    await _chatService.RemoveUserFromChatAsync(chatId, userId);
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
                    var chat = await _chatService.GetChatAsync(chatId);
                    if (chat.CreatedByUserId == userId)
                    {
                        await DeleteChat(chatId);
                    }
                    await Clients.Group(chatId.ToString()).SendAsync("ReceiveNotification", $"{userId} has left the chat");
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveError", ex.Message);
            }
        }
        public async Task DeleteChat(int chatId)
        {
            try
            {
                var user = Context.GetHttpContext().Request.Headers["UserId"];
                if (int.TryParse(user, out int userId))
                {

                    var users = await _userService.GetUsersByChatId(chatId);
                    var userIds = users.Select(u => u.Id).ToList();
                    await _chatService.DeleteChatAsync(chatId, userId);

                    var connections = UserConnections.Where(x => userIds.Contains(x.Value)).Select(x => x.Key);

                    foreach (var connectionId in connections)
                    {
                        await Groups.RemoveFromGroupAsync(connectionId, chatId.ToString());
                        await Clients.Client(connectionId).SendAsync("ReceiveNotification", $"You've been disconected from chat: {chatId}");
                    }
                }
            }
            catch(Exception ex)
            {
                await Clients.Caller.SendAsync($"{ex.Message}");
            }
        }
    }
}
