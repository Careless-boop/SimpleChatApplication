using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleChatApplication.BLL.DTOs;
using SimpleChatApplication.DAL;
using SimpleChatApplication.DAL.Entities;
using System.Net.Http.Json;

namespace SimpleChatApplication.XIntegTest
{
    public class ChatControllerIntegrationTests
    {
        [Fact]
        public async Task CreateChat_ReturnsCreatedChat()
        {
            // Arrange
            var application = new SimpleChatApplicationFactory();
            ChatDTO request = new ChatDTO
            {
                Id = 0,
                Name = "Test",
                CreatedByUserId = 1
            };

            var client = application.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/Chat/CreateChat", request);

            // Assert
            response.EnsureSuccessStatusCode();

            var chatResponse = await response.Content.ReadFromJsonAsync<ChatDTO>();
            chatResponse?.Id.Should().Be(1);
            chatResponse?.Name.Should().Be("Test");
            chatResponse?.CreatedByUserId.Should().Be(1);
        }

        [Fact]
        public async Task DeleteChat_ShouldReturnSuccess()
        {
            // Arrange
            var application = new SimpleChatApplicationFactory();
            ChatDTO request = new ChatDTO
            {
                Id = 0,
                Name = "Test",
                CreatedByUserId = 1
            };

            var client = application.CreateClient();
            using (var scope = application.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SimpleChatAppDbContext>();
                context!.Chats.Add(
                new Chat
                {
                    Name = "Test",
                    CreatedByUserId = 1,
                });

                await context.SaveChangesAsync();
            }

            // Act
            var response = await client.DeleteAsync("/Chat/DeleteChat/1?userId=1");

            // Assert
            response.EnsureSuccessStatusCode();

            var chatResponse = await response.Content.ReadFromJsonAsync<ChatDTO>();
            chatResponse?.Id.Should().Be(1);
            chatResponse?.Name.Should().Be("Test");
            chatResponse?.CreatedByUserId.Should().Be(1);
        }
    }

}
