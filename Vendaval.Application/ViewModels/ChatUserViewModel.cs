using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Enums;

namespace Vendaval.Application.ViewModels
{
    public class ChatUserViewModel : BaseModelViewModel
    {
        public required string ConnectionId { get; set; }
        public UserType UserType { get; set; }

        public required string Email { get; set; }

        public required string Name { get; set; }
    }
}
