using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimpleChatApplication.DAL;

namespace SimpleChatApplication.XIntegTest
{
    internal class SimpleChatApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<SimpleChatAppDbContext>));

                services.AddSqlServer<SimpleChatAppDbContext>("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True;");

                var dbContext = CreateDbContext(services);
                dbContext.Database.EnsureDeleted();
            });

        }
        private static SimpleChatAppDbContext CreateDbContext(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleChatAppDbContext>();
            return dbContext;
        }
    }
}