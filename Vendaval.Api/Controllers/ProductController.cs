using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendaval.Application.Services.Interfaces;
using Vendaval.Application.ViewModels;

namespace Vendaval.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductViewModelService _productViewModelService;

        public ProductController(IProductViewModelService productViewModelService)
        {
            _productViewModelService = productViewModelService;
        }

        [Authorize(Roles = "Admin, Seller")]
        [HttpPost("registerProduct")]
        public async Task<IActionResult> RegisterProduct([FromBody] ProductViewModel productViewModel)
        {
            try
            {
                var result = await _productViewModelService.RegisterProduct(productViewModel);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var result = await _productViewModelService.GetAllProducts();

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
