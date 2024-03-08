using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using Vendaval.Infrastructure.Data.Repositories.RedisRepositories.Interfaces;
using Oci.Common.Auth;
using Oci.Common;
using Oci.ObjectstorageService;

namespace Vendaval.Application.Services
{
    public class ProductViewModelService : IProductViewModelService
    {
        private readonly IProductRepository _productRepository;
        private readonly IRedisRepository _redisRepository;
        private readonly IMapper _mapper;
        private readonly string _baseUri;
        private readonly string _nameSpace;
        private readonly string _bucketName;
        private readonly string _tenancyId;
        private readonly string _userId;
        private readonly string _fingerprint;
        private readonly string _privateKeyPath;
        private readonly string _ociConfigPath;
        public ProductViewModelService(IProductRepository productRepository, IRedisRepository redisRepository, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IMapper mapper) 
        {
            _productRepository = productRepository;
            _redisRepository = redisRepository;
            _mapper = mapper;

            if (configuration == null )
                throw new ArgumentNullException(nameof(configuration));
            
            _baseUri = configuration.GetSection("Oci:Storage:BaseUri").Value;
            _nameSpace = configuration.GetSection("Oci:Storage:Namespace").Value;
            _bucketName = configuration.GetSection("Oci:Storage:Bucket").Value;
            _tenancyId = configuration.GetSection("Oci:TenancyId").Value;
            _userId = configuration.GetSection("Oci:UserId").Value;
            _fingerprint = configuration.GetSection("Oci:Fingerprint").Value;
            _privateKeyPath = $"{webHostEnvironment.WebRootPath}/{configuration.GetSection("Oci:PrivateKeyName").Value}";
            _ociConfigPath = $"{webHostEnvironment.WebRootPath}/ociConfig";

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
                await SaveAndClearCache();
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

            if(!Enum.IsDefined(typeof(ProductType), productViewModel.Category))
                return new MethodResult<ProductViewModel> { Success = false, Message = "Invalid Category" };


            return new MethodResult<ProductViewModel> { Success = true};
        }

        public async Task<MethodResult<List<ProductViewModel>>> GetAllProducts()
        {
            try
            {
                var productsOnCache = await GetProductsFromRedis();

                if (productsOnCache.Success)
                    return productsOnCache;

                var products = await _productRepository.GetAllAsync();

                if (products == null || products.Count == 0)
                    return new MethodResult<List<ProductViewModel>> { Success = false, Message = "No products found" };

                var result = new MethodResult<List<ProductViewModel>> { Success = true, Message = $" {products.Count} Products found", data = _mapper.Map<List<ProductViewModel>>(products) };
                await _redisRepository.SetValueAsync("products", JsonConvert.SerializeObject(result));
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on get products", ex);
            }
        }

        public async Task<MethodResult<ProductViewModel>> GetProductById(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);

                if (product == null)
                    return new MethodResult<ProductViewModel> { Success = false, Message = "Product not found" };

                return new MethodResult<ProductViewModel> { Success = true, Message = "Product found", data = _mapper.Map<ProductViewModel>(product) };
            }
            catch (Exception ex)
            {
                throw new Exception("Error on get product by id", ex);
            }
        }

        public async Task<MethodResult<ProductViewModel>> UpdateProduct(ProductViewModel productViewModel)
        {
            if (await _productRepository.GetByIdAsync(productViewModel.Id) == null)
                return new MethodResult<ProductViewModel> { Success = false, Message = "Product not found" };

            var productValidation = IsProductValid(productViewModel);

            if (!productValidation.Success)
                return productValidation;

            var product = _mapper.Map<Product>(productViewModel);

            try
            {
                _productRepository.Update(product.Id ,product);
                await SaveAndClearCache();
                var productUpdated = await _productRepository.GetByIdAsync(product.Id);
                return new MethodResult<ProductViewModel> { Success = true, Message = "Product was updated successfuly", data = _mapper.Map<ProductViewModel>(productUpdated) };
            }
            catch (Exception ex)
            {
                throw new Exception("Error on update product", ex);
            }
        }


        public async Task<MethodResult<ProductViewModel>> DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return new MethodResult<ProductViewModel> { Success = false, Message = "Product not found" };

            try
            {
                _productRepository.Delete(product);
                await SaveAndClearCache();
                return new MethodResult<ProductViewModel> { Success = true, Message = "Product was deleted successfuly", data = _mapper.Map<ProductViewModel>(product) };
            }
            catch (Exception ex)
            {
                throw new Exception("Error on delete product", ex);
            }
        }

        private async Task<MethodResult<List<ProductViewModel>>> GetProductsFromRedis()
        {
            var products = await _redisRepository.GetValueAsync("products");

            if(products.IsNullOrEmpty)
                return new MethodResult<List<ProductViewModel>> { Success = false, Message = "No products found" };

            var productsList = JsonConvert.DeserializeObject<MethodResult<List<ProductViewModel>>>(products);

            if(productsList == null || productsList.data == null || productsList.data.Count == 0)
                return new MethodResult<List<ProductViewModel>> { Success = false, Message = "No products found" };

            return productsList;
        }
        private async Task SaveAndClearCache()
        {
            await _productRepository.Save();
            await _redisRepository.RemoveValueAsync("products");
        }

        private ObjectStorageClient CreateObjectStorageClient()
        {
            Environment.SetEnvironmentVariable("OCI_CONFIG_FILE", _ociConfigPath);
            var provider = new ConfigFileAuthenticationDetailsProvider("DEFAULT");
            return new ObjectStorageClient(provider, new ClientConfiguration());
        }


        public async Task<MethodResult<object>> UploadProductImage(int productId, IFormFile image)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if(product == null)
                return new MethodResult<object> { Success = false, Message = "Product not found" };

            var putObjectRequest = new Oci.ObjectstorageService.Requests.PutObjectRequest
            {
                NamespaceName = _nameSpace,
                BucketName = _bucketName,
                ObjectName = $"ProductId{productId}",
                PutObjectBody = image.OpenReadStream()
            };

            try
            {
                using (var client = CreateObjectStorageClient())
                {
                    var putObjectResponse = await client.PutObject(putObjectRequest);
                    return new MethodResult<object> { Success = true, Message = "Image uploaded successfuly" };
                }
            }

            catch (Exception ex)
            {
                return new MethodResult<object> { Success = false, Message = ex.Message };
            }
        }

        public async Task<MethodResult<object>> GetLinksToProductImages()
        {
            var createPreauthenticatedRequestDetails = new Oci.ObjectstorageService.Models.CreatePreauthenticatedRequestDetails
            {
                Name = "ProductImagesRequest",
                AccessType = Oci.ObjectstorageService.Models.CreatePreauthenticatedRequestDetails.AccessTypeEnum.AnyObjectRead,
                TimeExpires = DateTime.Now.AddDays(7),
                BucketListingAction = Oci.ObjectstorageService.Models.PreauthenticatedRequest.BucketListingActionEnum.ListObjects
            };

            var createPreauthenticatedRequestRequest = new Oci.ObjectstorageService.Requests.CreatePreauthenticatedRequestRequest
            {
                NamespaceName = _nameSpace,
                BucketName = _bucketName,
                CreatePreauthenticatedRequestDetails = createPreauthenticatedRequestDetails
            };

            try
            {
                using (var client = CreateObjectStorageClient())
                {
                    var createPreauthenticatedRequestResponse = await client.CreatePreauthenticatedRequest(createPreauthenticatedRequestRequest);
                    return new MethodResult<object> { Success = true, data = createPreauthenticatedRequestResponse.PreauthenticatedRequest };
                }
            }

            catch (Exception ex)
            {
                return new MethodResult<object> { Success = false, Message = ex.Message };
            }
        }



    }
}
