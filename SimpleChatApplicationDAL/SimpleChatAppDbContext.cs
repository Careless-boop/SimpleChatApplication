using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.DAL
{
    public class SimpleChatAppDbContext : DbContext
    {
        public SimpleChatAppDbContext()
        {
        }
        public SimpleChatAppDbContext(DbContextOptions<SimpleChatAppDbContext> options)
            : base(options) 
        { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserChat> UserChats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SimpleChatAppDbContext).Assembly);
        }
    }
}
