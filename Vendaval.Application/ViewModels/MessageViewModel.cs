using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.ValueObjects;

namespace Vendaval.Application.ViewModels
{
    public class MessageViewModel : BaseModelViewModel
    {
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public required string SenderConnectionId { get; set; }
        public required string ReceiverConnectionId { get; set; }
        public required string Content { get; set; }
        public List<MessageMedia>? Media { get; set; }
    }
}
