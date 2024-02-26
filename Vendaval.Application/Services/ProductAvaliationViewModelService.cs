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
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class ProductAvaliationViewModelService : IProductAvaliationViewModelService
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
            var productAvaliationValidation = await IsProductAvaliationValid(productAvaliationViewModel);

            if (!productAvaliationValidation.Success)
                return productAvaliationValidation;

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

        public async Task<MethodResult<ProductAvaliationViewModel>> UpdateProductAvaliation(ProductAvaliationViewModel productAvaliationViewModel)
        {
            

            var productAvaliation = _mapper.Map<ProductAvaliation>(productAvaliationViewModel);

            try
            {
                _productAvaliationRepository.Update(productAvaliation.Id,productAvaliation);
                await _productAvaliationRepository.Save();
                var productAvaliationUpdated = await _productAvaliationRepository.GetByIdAsync(productAvaliation.Id);
                return new MethodResult<ProductAvaliationViewModel> { Success = true, Message = "Product Avaliation was updated successfuly", data = _mapper.Map<ProductAvaliationViewModel>(productAvaliationUpdated) };
            }
            catch (Exception ex)
            {
                throw new Exception("Error on update product avaliation", ex);
            }
        }

        public MethodResult<List<ProductAvaliationViewModel>> GetAvaliationsByProductId(int productId)
        {
            var productAvaliations = _productAvaliationRepository.GetWhere( a => a.ProductId == productId);

            if (productAvaliations == null || productAvaliations.Count() == 0)
                return new MethodResult<List<ProductAvaliationViewModel>> { Success = false, Message = "No avaliations found" };

            return new MethodResult<List<ProductAvaliationViewModel>> { Success = true, Message = $" {productAvaliations.Count()} Avaliations found", data = _mapper.Map<List<ProductAvaliationViewModel>>(productAvaliations) };
        }



        private async Task <MethodResult<ProductAvaliationViewModel>> IsProductAvaliationValid(ProductAvaliationViewModel productAvaliationViewModel)
        {
            if (productAvaliationViewModel == null)
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Product Avaliation cannot be null" };

            if (productAvaliationViewModel.Id <= 0)
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Product Avaliation Id is required and should be greater than 0" };

            if (productAvaliationViewModel.ProductId <= 0)
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Product Id is required and should be greater than 0" };

            if (Enum.IsDefined(productAvaliationViewModel.Stars))
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Rating is required and should be between 1 and 5" };

            if (string.IsNullOrEmpty(productAvaliationViewModel.Description))
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Description is required" };

            if (string.IsNullOrEmpty(productAvaliationViewModel.CostumerName))
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Costumer Name is required" };

            if (string.IsNullOrEmpty(productAvaliationViewModel.Title))
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Title is required" };

            var product = await _productRepository.GetByIdAsync(productAvaliationViewModel.ProductId);

            if (product == null)
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Product not found" };

            return new MethodResult<ProductAvaliationViewModel> { Success = true, Message = "Product Avaliation is valid" };
        }

        public async Task<MethodResult<ProductAvaliationViewModel>> DeleteProductAvaliation(int productAvaliationId)
        {
            var productAvaliation = await _productAvaliationRepository.GetByIdAsync(productAvaliationId);

            if (productAvaliation == null)
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Product Avaliation not found" };

            try
            {
                _productAvaliationRepository.Delete(productAvaliation);
                await _productAvaliationRepository.Save();
                return new MethodResult<ProductAvaliationViewModel> { Success = true, Message = "Product Avaliation was deleted successfuly" };
            }
            catch (Exception ex)
            {
                throw new Exception("Error on delete product avaliation", ex);
            }
        }
    }
}
