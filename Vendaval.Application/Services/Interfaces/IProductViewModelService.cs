using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;

namespace Vendaval.Application.Services.Interfaces
{
    public interface IProductViewModelService
    {
        Task<MethodResult<ProductViewModel>> RegisterProduct(ProductViewModel productViewModel);
        Task<MethodResult<List<ProductViewModel>>> GetAllProducts();
        Task<MethodResult<ProductViewModel>> UpdateProduct(ProductViewModel productViewModel);
        Task<MethodResult<ProductViewModel>> GetProductById(int id);
        Task<MethodResult<ProductViewModel>> DeleteProduct(int id);
    }
}
