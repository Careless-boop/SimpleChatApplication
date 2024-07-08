using SimpleChatApplication.BLL.DTOs;

namespace SimpleChatApplication.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<MessageDTO> AddMessageToUser(int chatId, int userId, string message);
        Task<IEnumerable<UserDTO>> GetUsersByChatId(int chatId);
    }
}
