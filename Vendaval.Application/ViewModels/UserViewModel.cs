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
    public class UserViewModel
    {
        int Id { get; set; }

        UserType UserType { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório")]
        string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        string Password { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        string Name { get; set; }

        DateTime BirthDate { get; set; }

        List<Address>? Addresses { get; set; }
    }
}
