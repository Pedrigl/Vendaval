using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Domain.Entities
{
    public class Conversation : BaseModel
    {
        public required List<ChatUser> Participants { get; set; }
        public List<Message>? Messages { get; set; }
    }
}
