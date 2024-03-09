using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vendaval.Application.Services.Interfaces;
using Vendaval.Application.ViewModels;

namespace Vendaval.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderVieModelService _orderViewModelService;

        public OrderController(IOrderVieModelService orderViewModelService)
        {
            _orderViewModelService = orderViewModelService;
        }

        [Authorize]
        [HttpGet("getByUserId")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                var orders = await _orderViewModelService.GetOrdersByUserIdAsync(userId);

                if(orders.Success)
                {
                    return Ok(orders);
                }

                return BadRequest(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        [Authorize]
        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder(OrderViewModel orderViewModel)
        {
            try
            {
                var result = await _orderViewModelService.CreateOrderAsync(orderViewModel);

                if(result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut("updateOrder")]
        public async Task<IActionResult> UpdateOrderStatus(OrderViewModel order)
        {
            try
            {
                var result = await _orderViewModelService.UpdateOrderAsync(order);

                if(result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("deleteOrder")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {
                var result = await _orderViewModelService.DeleteOrderAsync(orderId);

                if(result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
