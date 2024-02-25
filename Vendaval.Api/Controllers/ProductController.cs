using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendaval.Application.Services.Interfaces;
using Vendaval.Application.ViewModels;

namespace Vendaval.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, Seller")]
    public class ProductController : ControllerBase
    {
        private readonly IProductViewModelService _productViewModelService;

        public ProductController(IProductViewModelService productViewModelService)
        {
            _productViewModelService = productViewModelService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterProduct([FromBody] ProductViewModel productViewModel)
        {
            var result = await _productViewModelService.RegisterProduct(productViewModel);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
