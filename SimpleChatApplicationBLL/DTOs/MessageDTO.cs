using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatApplication.BLL.DTOs
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentTime { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
    }
}
