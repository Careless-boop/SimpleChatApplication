namespace SimpleChatApplication.DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentTime { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User User { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
