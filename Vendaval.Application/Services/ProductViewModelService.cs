using AutoMapper;
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
using Vendaval.Application.ValueObjects.Oci;
using Vendaval.Application.ValueObjects.Oci.Enums;
using Vendaval.Application.ViewModels;
using Vendaval.Domain.Entities;
using Vendaval.Domain.Enums;
using Vendaval.Infrastructure.Data.Contexts;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;
using Vendaval.Infrastructure.Data.Repositories.RedisRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class ProductViewModelService : IProductViewModelService
    {
        private readonly IProductRepository _productRepository;
        private readonly IRedisRepository _redisRepository;
        private HttpClient _httpClient;
        private readonly IMapper _mapper;

        public ProductViewModelService(IProductRepository productRepository, IRedisRepository redisRepository, IConfiguration configuration, IMapper mapper) 
        {
            _productRepository = productRepository;
            _redisRepository = redisRepository;
            _mapper = mapper;
            if (configuration == null )
                throw new ArgumentNullException(nameof(configuration));

            SetupHttpClient(configuration);
        }

        private HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                    _httpClient = new HttpClient();

                return _httpClient;
            }

        }
        private void SetupHttpClient(IConfiguration configuration)
        {
            var ociConfig = configuration.GetSection("Oci");
            var storageConfig = ociConfig.GetSection("Storage");
            var baseUri = storageConfig.GetSection("BaseUri").Value;
            var nameSpace = storageConfig.GetSection("Namespace").Value;
            var bucketName = storageConfig.GetSection("Bucket").Value;

            var uri = new StringBuilder(baseUri,150);
            uri.Append("/n/");
            uri.Append(nameSpace);
            uri.Append("/b/");
            uri.Append(bucketName);
            uri.Append("/");
            HttpClient.BaseAddress = new Uri(uri.ToString());
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

        public async Task<MethodResult<object>> UploadProductImage(int productId, IFormFile image)
        {

            byte[] fileBytes;

            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
            
            HttpResponseMessage request;

            try
            {
                request = await HttpClient.PutAsync("o/ProductId"+productId.ToString(), new ByteArrayContent(fileBytes));

            }
            catch (Exception ex)
            {
                return new MethodResult<object> { Success = false, Message = ex.Message};
            }

            if (request.IsSuccessStatusCode)
                return new MethodResult<object> { Success = true, Message = "Image uploaded successfuly" };

            var response = await request.Content.ReadAsStringAsync();

            return new MethodResult<object> { Success = false, Message = "Error uploading image", data = JsonConvert.DeserializeObject<OciError>(response) };
        }

        private string CreateAuthorizationHeader(string requestMethod, string requestUri, string contentMd5, string contentType, string date)
        {
            var stringToSign = $"{requestMethod}\n{contentMd5}\n{contentType}\n{date}\n{requestUri}";
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes("secret"));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            return $"Signature {signature}";
        }

        public async Task<MethodResult<object>> GetLinksToProductImages()
        {
            var preauthRequestDetails = new CreatePreauthenticatedRequestDetails { 
                name = "ProductImageList", 
                timeExpires = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                accessType = AccessType.AnyObjectRead.ToString(),
                bucketListingAction = BucketListingAction.ListObjects.ToString()
            };
            
            try
            {
                var request = await HttpClient.PostAsync("p/", new StringContent(JsonConvert.SerializeObject(preauthRequestDetails), Encoding.UTF8,"application/json"));
                var response = await request.Content.ReadAsStringAsync();

                if (!request.IsSuccessStatusCode)
                    return new MethodResult<object> { Success = false, Message = "Error creating preauthenticated request", data = JsonConvert.DeserializeObject<OciError>(response) };

                var preauthRequest = JsonConvert.DeserializeObject<PreauthenticatedRequest>(response);

                return new MethodResult<object> { Success = true, Message = "Preauthenticated request created", data = preauthRequest};
            }
            catch (Exception ex)
            {
                return new MethodResult<object> { Success = false, Message = ex.Message};
            }
        }


    }
}
