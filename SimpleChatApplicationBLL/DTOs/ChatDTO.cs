using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatApplication.BLL.DTOs
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
