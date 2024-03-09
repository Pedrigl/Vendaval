using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Enums;

namespace Vendaval.Domain.Entities
{
    public class Order : BaseModel
    {
        public int CustomerId { get; set; }
        public required List<Product> Products { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
    }
}
