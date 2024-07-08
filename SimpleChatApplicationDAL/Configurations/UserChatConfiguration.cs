using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.DAL.Configurations
{
    public class UserChatConfiguration : IEntityTypeConfiguration<UserChat>
    {
        public void Configure(EntityTypeBuilder<UserChat> builder)
        {
            builder.HasKey(uc => new { uc.UserId, uc.ChatId });
        }
    }
}
