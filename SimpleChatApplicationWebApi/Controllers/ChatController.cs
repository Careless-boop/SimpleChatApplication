using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using SimpleChatApplication.BLL.DTOs;
using SimpleChatApplication.BLL.Services.Interfaces;
using SimpleChatApplication.DAL;

namespace SimpleChatApplication.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService, SimpleChatAppDbContext dbContext)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] ChatDTO chat)
        {
            try
            {
                var createdChat = await _chatService.CreateChatAsync(chat);

                return Ok(createdChat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChat(int id)
        {
            try
            {
                var chat = await _chatService.GetChatAsync(id);
                if (chat == null)
                {
                    return NotFound();
                }
                return Ok(chat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchChats([FromQuery] string searchTerm)
        {
            var chats = await _chatService.SearchChatsAsync(searchTerm);
            return Ok(chats);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id, [FromQuery] int userId)
        {
            try
            {
                var removedChat = await _chatService.DeleteChatAsync(id, userId);
                return Ok(removedChat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/{userId}")]
        public async Task<IActionResult> AddUserToChat(int id, int userId)
        {
            try
            {
                var user = await _chatService.AddUserToChatAsync(id, userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> UpdateChat([FromBody] ChatDTO chat)
        {
            try
            {
                var updatedChat = await _chatService.UpdateChatAsync(chat);
                return Ok(updatedChat);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/{userId}")]
        public async Task<IActionResult> RemoveUserFromChat(int id, int userId)
        {
            try
            {
                var user = await _chatService.RemoveUserFromChatAsync(id, userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
