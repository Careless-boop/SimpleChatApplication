using SimpleChatApplication.BLL.DTOs;

namespace SimpleChatApplication.BLL.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatDTO> CreateChatAsync(ChatDTO chat);
        Task<ChatDTO> GetChatAsync(int chatId);
        Task<IEnumerable<ChatDTO>> SearchChatsAsync(string searchKeyword);
        Task<ChatDTO> DeleteChatAsync(int chatId, int userId);
        Task<UserDTO> AddUserToChatAsync(int chatId, int userId);
        Task<UserDTO> RemoveUserFromChatAsync(int chatId, int userId);
        Task<ChatDTO> UpdateChatAsync(ChatDTO chat);
        Task<IEnumerable<ChatDTO>> GetChatsByUserId(int userId);
    }
}
