using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Enums;

namespace Vendaval.Application.ViewModels
{
    public class OrderViewModel : BaseModelViewModel
    {
        public int CustomerId { get; set; }
        public required List<ProductViewModel> Products { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
    }
}
