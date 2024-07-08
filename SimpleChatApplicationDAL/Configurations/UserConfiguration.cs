using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(60);

            builder.HasMany(u => u.Chats)
                    .WithMany(c => c.Users)
                    .UsingEntity<UserChat>(
                    uc => uc.HasOne(c => c.Chat).WithMany().HasForeignKey(c => c.ChatId),
                    uc => uc.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId))
                    .ToTable("UserChats");

            builder.HasMany(u => u.Messages)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        } 
    }
}
