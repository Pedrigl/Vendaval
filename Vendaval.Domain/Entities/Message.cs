using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.ValueObjects;

namespace Vendaval.Domain.Entities
{
    public class Message : BaseModel
    {
        public int ConversationId { get; set; }
        public required string SenderConnectionId { get; set; }
        public required string ReceiverConnectionId { get; set; }
        public required string Content { get; set; }
        public List<MessageMedia>? Media { get; set; }
    }
}
