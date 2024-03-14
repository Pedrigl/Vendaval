using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Application.ViewModels
{
    public class ConversationViewModel : BaseModelViewModel
    {
        public required List<ChatUserViewModel> Participants { get; set; }
        public List<MessageViewModel>? Messages { get; set; }
    }
}
