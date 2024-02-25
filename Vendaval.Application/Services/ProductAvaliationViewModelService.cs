using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;
using Vendaval.Domain.Entities;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class ProductAvaliationViewModelService
    {
        private readonly IProductAvaliationRepository _productAvaliationRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductAvaliationViewModelService(IProductAvaliationRepository productAvaliationRepository, IProductRepository productRepository, IMapper mapper)
        {
            _productAvaliationRepository = productAvaliationRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<MethodResult<ProductAvaliationViewModel>> RegisterProductAvaliation(ProductAvaliationViewModel productAvaliationViewModel)
        {
            if (productAvaliationViewModel == null)
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Product Avaliation cannot be null" };

            if (productAvaliationViewModel.ProductId <= 0)
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Product Id is required and should be greater than 0" };

            if (Enum.IsDefined(productAvaliationViewModel.Stars))
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Rating is required and should be between 1 and 5" };

            var product = await _productRepository.GetByIdAsync(productAvaliationViewModel.ProductId);

            if (product == null)
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Product not found" };

            var productAvaliation = _mapper.Map<ProductAvaliation>(productAvaliationViewModel);

            try
            {
                var productAvaliationAdded = await _productAvaliationRepository.AddAsync(productAvaliation);
                return new MethodResult<ProductAvaliationViewModel> { Success = true, Message = "Product Avaliation was added successfuly", data = _mapper.Map<ProductAvaliationViewModel>(productAvaliationAdded) };
            }
            catch (Exception ex)
            {
                throw new Exception("Error on register product avaliation", ex);
            }
        }
    }
}
