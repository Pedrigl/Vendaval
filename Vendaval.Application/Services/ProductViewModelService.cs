using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.Services.Interfaces;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;
using Vendaval.Domain.Entities;
using Vendaval.Domain.Enums;
using Vendaval.Infrastructure.Data.Contexts;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class ProductViewModelService : IProductViewModelService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductAvaliationRepository _productAvaliationRepository;
        private readonly IMapper _mapper;
        public ProductViewModelService(IProductRepository productRepository, IProductAvaliationRepository productAvaliationRepository, IMapper mapper) 
        {
            _productRepository = productRepository;
            _productAvaliationRepository = productAvaliationRepository;
            _mapper = mapper;
        }

        public async Task<MethodResult<ProductViewModel>> RegisterProduct(ProductViewModel productViewModel)
        {
            var productValidation = IsProductValid(productViewModel);

            if (!productValidation.Success)
                return productValidation;

            var product = _mapper.Map<Product>(productViewModel);

            try
            {
                var productAdded = await _productRepository.AddAsync(product);
                return new MethodResult<ProductViewModel> { Success = true, Message = "Product was added successfuly" , data = _mapper.Map<ProductViewModel>(productAdded) };
            }
            catch (Exception ex)
            {
                throw new Exception("Error on register product", ex);
            }

        }

        private MethodResult<ProductViewModel> IsProductValid(ProductViewModel productViewModel)
        {
            if(productViewModel == null)
                return new MethodResult<ProductViewModel> { Success = false, Message = "Product cannot be null" };

            if (productViewModel.Name == null)
                return new MethodResult<ProductViewModel> { Success = false, Message = "Name is required" };

            if (productViewModel.Description == null)
                return new MethodResult<ProductViewModel> { Success = false, Message = "Description is required" };

            if (productViewModel.Price <= 0)
                return new MethodResult<ProductViewModel> { Success = false, Message = "Price is required and should be greater than 0" };

            if(!Enum.IsDefined(typeof(ProductType), productViewModel.CategoryId))
                return new MethodResult<ProductViewModel> { Success = false, Message = "Invalid Category" };


            return new MethodResult<ProductViewModel> { Success = true};
        }

        public async Task<MethodResult<List<ProductViewModel>>> GetAllProducts()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();

                if (products == null || products.Count == 0)
                    return new MethodResult<List<ProductViewModel>> { Success = false, Message = "No products found" };

                return new MethodResult<List<ProductViewModel>> { Success = true, Message = "Products found", data = _mapper.Map<List<ProductViewModel>>(products) };
            }
            catch (Exception ex)
            {
                throw new Exception("Error on get products", ex);
            }
        }

    }
}
