using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.BLL.DTOs;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.DAL;
using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.XUnitTest.ChatServiceTests
{
    public class DeleteChatAsyncTests
    {
        [Fact]
        public async Task DeleteChatAsync_ShouldReturnChatDTO_WhenArgumentsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SimpleChatAppDbContext>()
            .UseInMemoryDatabase(databaseName: "ChatAppDB")
            .Options;

            using (var context = new SimpleChatAppDbContext(options))
            {
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Test"
                });
                context.Chats.Add(new Chat
                {
                    Id = 1,
                    Name = "TestChat",
                    CreatedByUserId = 1
                });
                await context.SaveChangesAsync();
            }

            //Act
            ChatDTO result;
            using (var context = new SimpleChatAppDbContext(options))
            {
                result = await new ChatService(context).DeleteChatAsync(1, 1);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("TestChat", result.Name);
            Assert.Equal(1, result.CreatedByUserId);
        }
        public async Task DeleteChatAsync_ShouldThrowException_WhenArgumentsChatMissing()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SimpleChatAppDbContext>()
            .UseInMemoryDatabase(databaseName: "ChatAppDB")
            .Options;

            using (var context = new SimpleChatAppDbContext(options))
            {
                context.Users.Add(new User
                {
                    Id = 1,
                    Username = "Test"
                });
                context.Chats.Add(new Chat
                {
                    Id = 2,
                    Name = "TestChat",
                    CreatedByUserId = 2
                });
                await context.SaveChangesAsync();
            }

            //Act
            ChatDTO result;
            using (var context = new SimpleChatAppDbContext(options))
            {
                //Assert
                Assert.ThrowsAsync<KeyNotFoundException>(x => new ChatService(context).DeleteChatAsync(1, 1));
            }
        }
    }

}