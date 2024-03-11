using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Application.ViewModels
{
    public class UserAddressViewModel : BaseModelViewModel
    {
        public int UserId { get; set; }
        public required string Street { get; set; }
        public required string Number { get; set; }
        public string? Complement { get; set; }
        public required string Neighborhood { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Country { get; set; }
        public required string ZipCode { get; set; }
    }
}
