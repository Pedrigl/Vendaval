using AutoMapper;
using Newtonsoft.Json;
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
    public class OrderViewModelService : IOrderVieModelService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRedisRepository _redisRepository;
        private readonly IMapper _mapper;

        public OrderViewModelService(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository, IRedisRepository redisRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _redisRepository = redisRepository;
            _mapper = mapper;
        }
        public async Task<MethodResult<List<OrderViewModel>>> GetAllOrders()
        {
            var ordersOnCache = await _redisRepository.GetValueAsync("UserId0Orders");

            if (!ordersOnCache.IsNullOrEmpty)
            {
                var ordersOnCacheParse = JsonConvert.DeserializeObject<List<Order>>(ordersOnCache);
                var orderOnCacheViewModel = _mapper.Map<List<OrderViewModel>>(ordersOnCacheParse);
                return new MethodResult<List<OrderViewModel>> { Success = true, data = orderOnCacheViewModel };
            }
            var orders = await _orderRepository.GetAllAsync();

            if (orders.Count == 0)
            {
                return new MethodResult<List<OrderViewModel>> { Success = false, Message = "No orders where found" };
            }

            var orderViewModel = _mapper.Map<List<OrderViewModel>>(orders);

            await SaveOrdersOnCache(0, orders);

            return new MethodResult<List<OrderViewModel>> { Success = true, data = orderViewModel };
        }
        public async Task<MethodResult<List<OrderViewModel>>> GetOrdersByUserIdAsync(int userId)
        {
            var ordersOnCache = await _redisRepository.GetValueAsync($"UserId{userId}Orders");

            if (!ordersOnCache.IsNullOrEmpty)
            {
                var orders = JsonConvert.DeserializeObject<List<Order>>(ordersOnCache);
                var orderOnCacheViewModel = _mapper.Map<List<OrderViewModel>>(orders);
                return new MethodResult<List<OrderViewModel>> { Success = true, data = orderOnCacheViewModel };
            }

            var order = _orderRepository.GetWhere(x => x.CustomerId == userId);

            if (order == null)
                return new MethodResult<List<OrderViewModel>> { Success = false, Message = "No orders where found for this user"};

            await SaveOrdersOnCache(userId, order);

            var orderViewModel = _mapper.Map<List<OrderViewModel>>(order);

            return new MethodResult<List<OrderViewModel>> { Success = true, data = orderViewModel };
        }

        public async Task<MethodResult<OrderViewModel>> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                return new MethodResult<OrderViewModel> { Success = false, Message = "No order was found with this id" };

            var orderViewModel = _mapper.Map<OrderViewModel>(order);

            return new MethodResult<OrderViewModel> { Success = true, data = orderViewModel };
        }

        public async Task<MethodResult<OrderViewModel>> CreateOrderAsync(OrderViewModel orderViewModel)
        {
            var order = _mapper.Map<Order>(orderViewModel);

            var user = await _userRepository.GetByIdAsync(order.CustomerId);

            if (user == null)
                return new MethodResult<OrderViewModel> { Success = false, Message = "No user was found with this id" };

            for (int i = 0; i < orderViewModel.Products.Count; i++)
            {
                var products = _productRepository.GetWhere(p => p.Id == orderViewModel.Products[i].Id);

                if (products.Any(p => p == null))
                    return new MethodResult<OrderViewModel> { Success = false, Message = "Some Products in this order where not found" };
            }

            var result = await _orderRepository.AddAsync(order);

            if (result == null)
                return new MethodResult<OrderViewModel> { Success = false, Message = "An error occurred while creating the order" };

            await SaveAndClearCache(order.CustomerId);

            var orderViewModelResult = _mapper.Map<OrderViewModel>(result);

            return new MethodResult<OrderViewModel> { Success = true, data = orderViewModelResult };
        }

        public async Task<MethodResult<OrderViewModel>> UpdateOrderAsync(OrderViewModel orderViewModel)
        {
            var order = _mapper.Map<Order>(orderViewModel);

            var user = await _userRepository.GetByIdAsync(order.CustomerId);

            if (user == null)
                return new MethodResult<OrderViewModel> { Success = false, Message = "No user was found with this id" };

            for (int i = 0; i < orderViewModel.Products.Count; i++)
            {
                var products = _productRepository.GetWhere(p => p.Id == orderViewModel.Products[i].Id);

                if (products.Any(p => p == null))
                    return new MethodResult<OrderViewModel> { Success = false, Message = "Some Products in this order where not found" };
            }

            try
            {
                _orderRepository.Update(order.Id, order);

                await SaveAndClearCache(order.CustomerId);
            }
            catch (Exception ex)
            {
                return new MethodResult<OrderViewModel> { Success = false, Message = $"An error occurred while updating the order: {ex.Message}" };
            }

            var orderViewModelResult = _mapper.Map<OrderViewModel>(await GetOrderByIdAsync(order.Id));

            return new MethodResult<OrderViewModel> { Success = true, data = orderViewModelResult };
        }

        public async Task<MethodResult<object>> DeleteOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                return new MethodResult<object> { Success = false, Message = "No order was found with this id" };

            try
            {
                _orderRepository.Delete(order);
                await SaveAndClearCache(order.CustomerId);
                await SaveAndClearCache(order.CustomerId);
            }
            catch (Exception ex)
            {
                return new MethodResult<object> { Success = false, Message = $"An error occurred while deleting the order: {ex.Message}" };
            }

            return new MethodResult<object> { Success = true, Message = "The order was deleted successfully" };
        }

        private async Task SaveOrdersOnCache(int userId, IEnumerable<Order> orders)
        {
            await _redisRepository.SetValueAsync($"UserId{userId}Orders",JsonConvert.SerializeObject(orders));
        }

        private async Task<bool> SaveAndClearCache(int userId)
        {
            var didSave = await _orderRepository.Save();
            await _redisRepository.RemoveValueAsync($"UserId{userId}Orders");
            return didSave;
        }
    }
}
