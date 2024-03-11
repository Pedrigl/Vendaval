using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Enums;

namespace Vendaval.Application.ViewModels
{
    public class ProductViewModel : BaseModelViewModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Image { get; set; }
        public ProductType Category { get; set; }
        public float Avaliation { get; set; }
    }
}
