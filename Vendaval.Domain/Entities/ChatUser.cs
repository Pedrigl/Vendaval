using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Enums;

namespace Vendaval.Domain.Entities
{
    public class ChatUser : BaseModel
    {
        public string ConnectionId { get; set; }
        public UserType UserType { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsOnline { get; set; }
    }
}
