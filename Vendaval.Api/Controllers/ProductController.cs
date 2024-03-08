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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Seller")]
        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductViewModel productViewModel)
        {
            try
            {
                var result = await _productViewModelService.UpdateProduct(productViewModel);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getLinkToProductImages")]
        public async Task<IActionResult> GetLinksToProductImages()
        {
            try
            {
                var result = await _productViewModelService.GetLinksToProductImages();

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("uploadProductImage")]
        public async Task<IActionResult> UploadProductImage(IFormFile image)
        {
            try
            {
                var result = await _productViewModelService.UploadProductImage(image);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var result = await _productViewModelService.GetProductById(id);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Seller")]
        [HttpDelete("deleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productViewModelService.DeleteProduct(id);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
