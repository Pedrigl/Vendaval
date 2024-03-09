using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Enums;
using Vendaval.Domain.ValueObjects;

namespace Vendaval.Domain.Entities
{
    public class ProductAvaliation : BaseModel
    {
        public int ProductId { get; set; }
        public string CostumerName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProductAvaliationStars Stars { get; set; }
        public List<AvaliationMedia>? Media { get; set; }

    }
}
