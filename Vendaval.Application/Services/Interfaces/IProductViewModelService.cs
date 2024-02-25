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
        Task<MethodResult<List<ProductViewModel>>> GetProducts();
    }
}
