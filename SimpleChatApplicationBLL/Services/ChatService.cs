using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.BLL.DTOs;
using SimpleChatApplication.BLL.Services.Interfaces;
using SimpleChatApplication.DAL;
using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.BLL.Services
{
    public class ChatService : IChatService
    {
        private readonly SimpleChatAppDbContext _context;

        public ChatService(SimpleChatAppDbContext context)
        {
            _context = context;
        }

        public async Task<ChatDTO> CreateChatAsync(ChatDTO chat)
        {
                var user = await _context.Users.FindAsync(chat.CreatedByUserId);
                if (user == null)
                {
                    throw new InvalidDataException($"Not found user with id: {chat.CreatedByUserId}");
                }
                var newChat = new Chat
                {
                    Name = chat.Name,
                    CreatedByUserId = chat.CreatedByUserId,
                    Users = { user }
                };
                await _context.Chats.AddAsync(newChat);
                await _context.SaveChangesAsync();
                chat.Id = newChat.Id;
                return chat;
        }
        public async Task<ChatDTO> GetChatAsync(int chatId)
        {
            var chat = await _context.Chats
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
            {
                throw new KeyNotFoundException($"Not found chat with id: {chatId}!");
            }

            return new ChatDTO
            {
                Id = chat.Id,
                Name = chat.Name,
                CreatedByUserId = chat.CreatedByUserId
            };
        }
        public async Task<IEnumerable<ChatDTO>> SearchChatsAsync(string searchKeyword)
        {
            var chats = await _context.Chats
                .Where(c => c.Name.Contains(searchKeyword))
                .ToListAsync();

            return chats.Select(chat => new ChatDTO
            {
                Id = chat.Id,
                Name = chat.Name,
                CreatedByUserId = chat.CreatedByUserId
            });
        }
        public async Task<ChatDTO> DeleteChatAsync(int chatId, int userId)
        {
            var chat = await _context.Chats.FindAsync(chatId);

            if (chat == null)
            {
                throw new KeyNotFoundException($"Not found chat with id: {chatId}");
            }

            if (chat.CreatedByUserId != userId)
            {
                throw new UnauthorizedAccessException("There are no permissions to do the operation.");
            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();

            return new ChatDTO
            {
                Id = chat.Id,
                Name = chat.Name,
                CreatedByUserId = chat.CreatedByUserId
            };
        }
        public async Task<UserDTO> AddUserToChatAsync(int chatId, int userId)
        {
            var chat = await _context.Chats.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == chatId);
            var user = await _context.Users.FindAsync(userId);

            if (chat == null)
            {
                throw new KeyNotFoundException("Chat not found.");
            }

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (!chat.Users.Contains(user))
            {
                chat.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username
            };
        }
        public async Task<UserDTO> RemoveUserFromChatAsync(int chatId, int userId)
        {
            var chat = await _context.Chats.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == chatId);
            var user = await _context.Users.FindAsync(userId);

            if (chat == null)
            {
                throw new KeyNotFoundException("Chat not found.");
            }

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (chat.Users.Contains(user))
            {
                chat.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidDataException($"Not found user in this chat: {chatId}");
            }

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username
            };
        }
        public async Task<ChatDTO> UpdateChatAsync(ChatDTO chat)
        {
            var updatingChat = await _context.Chats.FindAsync(chat.Id);
            if (updatingChat == null)
            {
                throw new KeyNotFoundException("Chat not found.");
            }
            updatingChat.Name = chat.Name;
            _context.Chats.Update(updatingChat);

            return chat;
        }
        public async Task<IEnumerable<ChatDTO>> GetChatsByUserId(int userId)
        {
            var chats = _context.Chats.Include(c => c.Users).Where(c => c.Users.Any(u => u.Id == userId));
            return  await chats.Select(c =>
                new ChatDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedByUserId = c.CreatedByUserId
                }).ToListAsync();
        }
    }
}
