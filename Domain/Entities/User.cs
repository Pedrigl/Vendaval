using Vendaval.Server.Domain.Enums;
using Vendaval.Server.Domain.ValueObjects;

namespace Vendaval.Server.Domain.Entities
{
    public class User : BaseModel
    {
        public UserType UserType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public Address? Address { get; set; }
    }
}
