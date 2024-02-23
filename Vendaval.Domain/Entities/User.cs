using Vendaval.Domain.Enums;
using Vendaval.Domain.ValueObjects;

namespace Vendaval.Domain.Entities
{
    public class User : BaseModel
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
