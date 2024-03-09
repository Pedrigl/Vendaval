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

        public string Email { get; set; }
        
        public string Password { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public List<Address>? Address { get; set; }

    }
}
