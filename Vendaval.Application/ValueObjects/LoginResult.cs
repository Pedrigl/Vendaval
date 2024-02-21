using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ViewModels;

namespace Vendaval.Application.ValueObjects
{
    public class LoginResult
    {
        public bool Success { get; set; } = true;
        public string? Token { get; set; }
        public string? Message { get; set; }
        public UserViewModel? User { get; set; }
    }
}
