using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Enums;
using Vendaval.Domain.ValueObjects;

namespace Vendaval.Application.ViewModels
{
    public class UserViewModel: BaseModelViewModel
    {
        public UserType UserType { get; set; }

        public required string Email { get; set; }
        
        public required string Password { get; set; }

        public required string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public required string PhoneNumber { get; set; }

    }
}
