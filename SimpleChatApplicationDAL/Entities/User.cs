namespace SimpleChatApplication.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<Chat> Chats { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
    }
}
