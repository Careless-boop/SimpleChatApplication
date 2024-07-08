using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.DAL;
using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.WebApi.Extensions
{
    public static class SeedingLocalExtension
    {
        public static async Task ApplyMigrations(this WebApplication app)
        {
            try
            {
                var dbContext = app.Services.GetRequiredService<SimpleChatAppDbContext>();
                
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
        public static async Task SeedDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SimpleChatAppDbContext>();
                await dbContext.Database.MigrateAsync();

                if (!dbContext.Users.Any())
                {
                    await dbContext.Users.AddRangeAsync(new List<User>
                    {
                        new User
                        {
                            Username = "firstuser"
                        },
                        new User
                        {
                            Username = "seconduser"
                        },
                        new User
                        {
                            Username = "thirduser"
                        },
                        new User
                        {
                            Username = "forthuser"
                        },
                    });

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
