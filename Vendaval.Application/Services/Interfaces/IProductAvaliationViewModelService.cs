using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;

namespace Vendaval.Application.Services.Interfaces
{
    public interface IProductAvaliationViewModelService
    {
        MethodResult<List<ProductAvaliationViewModel>> GetAvaliationsByProductId(int productId);
        Task<MethodResult<ProductAvaliationViewModel>> RegisterProductAvaliation(ProductAvaliationViewModel productAvaliationViewModel);
        Task<MethodResult<ProductAvaliationViewModel>> UpdateProductAvaliation(ProductAvaliationViewModel productAvaliationViewModel);
        Task<MethodResult<ProductAvaliationViewModel>> DeleteProductAvaliation(int productAvaliationId);
    }
}
