using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Enums;
using Vendaval.Domain.ValueObjects;

namespace Vendaval.Application.ViewModels
{
    public class ProductAvaliationViewModel : BaseModelViewModel
    {
        public int ProductId { get; set; }
        public required string CostumerName { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<AvaliationMedia>? Media { get; set; }
        public ProductAvaliationStars Stars { get; set; }
    }
}
