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
using Oci.ObjectstorageService.Models;
using Oci.ObjectstorageService.Responses;

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
                await DeleteProductImage(product.Name);
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
        public async Task<MethodResult<DeleteObjectResponse>> DeleteProductImage(string productName)
        {
            var deleteObjectRequest = new Oci.ObjectstorageService.Requests.DeleteObjectRequest
            {
                NamespaceName = _nameSpace,
                BucketName = _bucketName,
                ObjectName = $"ProductName{productName}"
            };

            try
            {
                using (var client = CreateObjectStorageClient())
                {
                    var deleteObjectResponse = await client.DeleteObject(deleteObjectRequest);
                    return new MethodResult<DeleteObjectResponse> { Success = true, Message = "Image deleted successfuly", data = deleteObjectResponse };
                }
            }

            catch (Exception ex)
            {
                return new MethodResult<DeleteObjectResponse> { Success = false, Message = ex.Message };
            }
        }

        private bool IsImageValid(IFormFile image)
        {
            if (image == null)
                return false;

            if (image.Length == 0)
                return false;

            if (!image.ContentType.Contains("image"))
                return false;

            return true;
        }

        public async Task<MethodResult<object>> UploadProductImage(string productName, IFormFile image)
        {
            if (!IsImageValid(image))
                return new MethodResult<object> { Success = false, Message = "Invalid image" };

            var putObjectRequest = new Oci.ObjectstorageService.Requests.PutObjectRequest
            {
                NamespaceName = _nameSpace,
                BucketName = _bucketName,
                ObjectName = $"ProductName{productName}",
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

        public async Task<MethodResult<PreauthenticatedRequest>> GetLinksToProductImages()
        {
            var createPreauthenticatedRequestDetails = new CreatePreauthenticatedRequestDetails
            {
                Name = "ProductImagesRequest",
                AccessType = CreatePreauthenticatedRequestDetails.AccessTypeEnum.AnyObjectRead,
                TimeExpires = DateTime.Now.AddDays(7),
                BucketListingAction = PreauthenticatedRequest.BucketListingActionEnum.ListObjects
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
                    return new MethodResult<PreauthenticatedRequest> { Success = true, data = createPreauthenticatedRequestResponse.PreauthenticatedRequest };
                }
            }

            catch (Exception ex)
            {
                return new MethodResult<PreauthenticatedRequest> { Success = false, Message = ex.Message };
            }
        }



    }
}
