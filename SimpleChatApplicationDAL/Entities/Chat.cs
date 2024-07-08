namespace SimpleChatApplication.DAL.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public List<Message> Messages { get; set; } = new();
        public List<User> Users { get; set; } = new();
    }
}
