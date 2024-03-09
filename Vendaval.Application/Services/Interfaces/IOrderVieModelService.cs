using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;

namespace Vendaval.Application.Services.Interfaces
{
    public interface IOrderVieModelService
    {
        Task<MethodResult<List<OrderViewModel>>> GetOrdersByUserIdAsync(int userId);
        Task<MethodResult<OrderViewModel>> GetOrderByIdAsync(int orderId);
        Task<MethodResult<OrderViewModel>> CreateOrderAsync(OrderViewModel orderViewModel);
        Task<MethodResult<OrderViewModel>> UpdateOrderAsync(OrderViewModel orderViewModel);
        Task<MethodResult<object>> DeleteOrderAsync(int orderId);
    }
}
