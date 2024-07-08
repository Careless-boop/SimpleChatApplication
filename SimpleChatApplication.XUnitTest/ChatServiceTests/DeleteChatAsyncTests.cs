using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.BLL.DTOs;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.DAL;
using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.XUnitTest.ChatServiceTests
{
    public class DeleteChatAsyncTests
    {
        private SimpleChatAppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<SimpleChatAppDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;

            var dbContext = new SimpleChatAppDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        [Fact]
        public async Task DeleteChatAsync_ShouldReturnChatDTO_WhenArgumentsValid()
        {
            // Arrange
            ChatDTO result;
            using (var dbContext = CreateDbContext())
            {
                dbContext.Users.Add(new User
                {
                    Id = 1,
                    Username = "Test"
                });
                dbContext.Chats.Add(new Chat
                {
                    Id = 1,
                    Name = "TestChat",
                    CreatedByUserId = 1
                });
                await dbContext.SaveChangesAsync();

                result = await new ChatService(dbContext).DeleteChatAsync(1, 1);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("TestChat", result.Name);
            Assert.Equal(1, result.CreatedByUserId);
        }

        [Fact]
        public async Task DeleteChatAsync_ShouldThrowException_WhenChatNotFound()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.Users.Add(new User
                {
                    Id = 1,
                    Username = "Test"
                });
                dbContext.Chats.Add(new Chat
                {
                    Id = 2,
                    Name = "TestChat",
                    CreatedByUserId = 2
                });
                await dbContext.SaveChangesAsync();


                // Act & Assert
                await Assert.ThrowsAsync<KeyNotFoundException>(async () => await new ChatService(dbContext).DeleteChatAsync(1, 1));
            }
        }

        [Fact]
        public async Task DeleteChatAsync_ShouldThrowException_WhenUserUnauthorized()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.Users.Add(new User
                {
                    Id = 1,
                    Username = "Test"
                });
                dbContext.Users.Add(new User
                {
                    Id = 2,
                    Username = "OtherUser"
                });
                dbContext.Chats.Add(new Chat
                {
                    Id = 1,
                    Name = "TestChat",
                    CreatedByUserId = 2
                });
                await dbContext.SaveChangesAsync();

                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await new ChatService(dbContext).DeleteChatAsync(1, 1));
            }
        }
    }
}