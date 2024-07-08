using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.BLL.DTOs;
using SimpleChatApplication.BLL.Services.Interfaces;
using SimpleChatApplication.DAL;
using SimpleChatApplication.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatApplication.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly SimpleChatAppDbContext _context;
        public UserService(SimpleChatAppDbContext context) 
        {
            _context = context;
        }

        public async Task<MessageDTO> AddMessageToUser(int chatId, int userId, string message)
        {
            var user = await _context.Users.FindAsync(userId);
            var chat = await _context.Chats
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null || !chat.Users.Any(u => u.Id == userId))
            {
                throw new KeyNotFoundException("Chat not found.");
            }
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var newMessage = new Message
            {
                Content = message,
                UserId = userId,
                ChatId = chatId
            };

            user.Messages.Add(newMessage);

            await _context.SaveChangesAsync();

            return new MessageDTO
            {
                Id = newMessage.Id,
                Content = newMessage.Content,
                SentTime = newMessage.SentTime,
                UserId = newMessage.UserId,
                ChatId = newMessage.ChatId
            };
        }

        public async Task<IEnumerable<UserDTO>> GetUsersByChatId(int chatId)
        {
            var chat = await _context.Chats.FindAsync(chatId);
            if (chat == null)
            {
                throw new KeyNotFoundException("Chat not found.");
            }

            var users = _context.Users
                .Include(u => u.Chats)
                .Where(u => u.Chats
                .Contains(chat))
                .Select(u =>
                    new UserDTO
                    {
                        Id = u.Id,
                        Username = u.Username
                    });

            return users;
        }
    }
}
