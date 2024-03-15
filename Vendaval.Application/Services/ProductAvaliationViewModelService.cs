using AutoMapper;
using Newtonsoft.Json;
using StackExchange.Redis;
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
using Vendaval.Infrastructure.Data.Repositories.RedisRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class ProductAvaliationViewModelService : IProductAvaliationViewModelService
    {
        private readonly IProductAvaliationRepository _productAvaliationRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRedisRepository _redisRespotiroy;
        private readonly IMapper _mapper;

        public ProductAvaliationViewModelService(IProductAvaliationRepository productAvaliationRepository, IProductRepository productRepository,IRedisRepository redisRepository, IMapper mapper)
        {
            _productAvaliationRepository = productAvaliationRepository;
            _productRepository = productRepository;
            _redisRespotiroy = redisRepository;
            _mapper = mapper;
        }

        public async Task<MethodResult<ProductAvaliationViewModel>> RegisterProductAvaliation(ProductAvaliationViewModel productAvaliationViewModel)
        {
            var product = await _productRepository.GetByIdAsync(productAvaliationViewModel.ProductId);

            var productAvaliationValidation = IsProductAvaliationValid(productAvaliationViewModel, product);

            if (!productAvaliationValidation.Success)
                return productAvaliationValidation;

            var productAvaliation = _mapper.Map<ProductAvaliation>(productAvaliationViewModel);

            try
            {
                var avaliations = await GetAvaliationsByProductId(product.Id);
                var avaliationsCount = avaliations.data != null ? avaliations.data.Count : 0;
                var productAvaliationAdded = await _productAvaliationRepository.AddAsync(productAvaliation);
                IncrementAverageProductAvaliation(product, avaliationsCount, productAvaliation.Stars.GetHashCode());
                await SaveAndClearCache(productAvaliation.ProductId);
                return new MethodResult<ProductAvaliationViewModel> { Success = true, Message = "Product Avaliation was added successfuly", data = _mapper.Map<ProductAvaliationViewModel>(productAvaliationAdded) };
            }
            catch (Exception ex)
            {
                throw new Exception("Error on register product avaliation", ex);
            }
        }

        private void IncrementAverageProductAvaliation(Product product, int avaliationCount, int stars)
        {

            product.Avaliation = (product.Avaliation + stars) / avaliationCount;
            _productRepository.Update(product.Id, product);
        }

        private async Task<MethodResult<List<ProductAvaliationViewModel>>> GetAvaliationsFromRedis(int productId)
        {
            var avaliations = await _redisRespotiroy.GetValueAsync($"productId:{productId}:Avaliations");

            if (avaliations.IsNullOrEmpty)
                return new MethodResult<List<ProductAvaliationViewModel>> { Success = false, Message = "No avaliations found" };

            var avaliationsList = JsonConvert.DeserializeObject<MethodResult<List<ProductAvaliationViewModel>>>(avaliations);

            if(avaliationsList == null || avaliationsList.data == null || avaliationsList.data.Count == 0)
                return new MethodResult<List<ProductAvaliationViewModel>> { Success = false, Message = "No avaliations found" };

            return new MethodResult<List<ProductAvaliationViewModel>> { Success = true, Message = $" {avaliationsList.data.Count} Avaliations found", data = avaliationsList.data };
        }

        private async Task SaveAndClearCache(int productId)
        {
            await _productRepository.Save();
            await _productAvaliationRepository.Save();
            await _redisRespotiroy.RemoveValueAsync($"productId:{productId}:Avaliations");
        }

        public async Task<MethodResult<ProductAvaliationViewModel>> UpdateProductAvaliation(ProductAvaliationViewModel productAvaliationViewModel)
        {
            

            var productAvaliation = _mapper.Map<ProductAvaliation>(productAvaliationViewModel);

            try
            {
                _productAvaliationRepository.Update(productAvaliation.Id,productAvaliation);
                await SaveAndClearCache(productAvaliationViewModel.ProductId);
                var productAvaliationUpdated = await _productAvaliationRepository.GetByIdAsync(productAvaliation.Id);
                return new MethodResult<ProductAvaliationViewModel> { Success = true, Message = "Product Avaliation was updated successfuly", data = _mapper.Map<ProductAvaliationViewModel>(productAvaliationUpdated) };
            }
            catch (Exception ex)
            {
                throw new Exception("Error on update product avaliation", ex);
            }
        }

        public async Task<MethodResult<List<ProductAvaliationViewModel>>> GetAvaliationsByProductId(int productId)
        {
            var avaliationsOnCache = await GetAvaliationsFromRedis(productId);

            if (avaliationsOnCache.Success)
                return avaliationsOnCache;

            var productAvaliations = _productAvaliationRepository.GetWhere(a => a.ProductId == productId).ToList();

            if (productAvaliations == null || productAvaliations.Count() == 0)
                return new MethodResult<List<ProductAvaliationViewModel>> { Success = false, Message = "No avaliations found" };

            return new MethodResult<List<ProductAvaliationViewModel>> { Success = true, Message = $" {productAvaliations.Count()} Avaliations found", data = _mapper.Map<List<ProductAvaliationViewModel>>(productAvaliations) };
        }



        private MethodResult<ProductAvaliationViewModel> IsProductAvaliationValid(ProductAvaliationViewModel productAvaliationViewModel, Product product)
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

            if (product == null)
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Product not found" };

            return new MethodResult<ProductAvaliationViewModel> { Success = true, Message = "Product Avaliation is valid" };
        }

        public async Task<MethodResult<ProductAvaliationViewModel>> DeleteProductAvaliation(int productId,int productAvaliationId)
        {
            var productAvaliation = await _productAvaliationRepository.GetByIdAsync(productAvaliationId);

            if (productAvaliation == null)
                return new MethodResult<ProductAvaliationViewModel> { Success = false, Message = "Product Avaliation not found" };

            try
            {
                _productAvaliationRepository.Delete(productAvaliation);
                await SaveAndClearCache(productId);
                return new MethodResult<ProductAvaliationViewModel> { Success = true, Message = "Product Avaliation was deleted successfuly" };
            }
            catch (Exception ex)
            {
                throw new Exception("Error on delete product avaliation", ex);
            }
        }
    }
}
