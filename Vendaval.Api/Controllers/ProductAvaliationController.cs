using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vendaval.Application.Services.Interfaces;
using Vendaval.Application.ViewModels;

namespace Vendaval.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAvaliationController : ControllerBase
    {
        IProductAvaliationViewModelService _productAvaliationViewModelService;

        public ProductAvaliationController(IProductAvaliationViewModelService productAvaliationViewModelService)
        {
            _productAvaliationViewModelService = productAvaliationViewModelService;
        }

        [HttpGet("getByProductId")]
        public IActionResult GetAvaliationsByProductId(int productId)
        {
            try
            {
                var result = _productAvaliationViewModelService.GetAvaliationsByProductId(productId);
                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterProductAvaliation([FromBody] ProductAvaliationViewModel productAvaliationViewModel)
        {
            try
            {
                var result = await _productAvaliationViewModelService.RegisterProductAvaliation(productAvaliationViewModel);
                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProductAvaliation([FromBody] ProductAvaliationViewModel productAvaliationViewModel)
        {
            try
            {
                var result = await _productAvaliationViewModelService.UpdateProductAvaliation(productAvaliationViewModel);
                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProductAvaliation(int id)
        {
            try
            {
                var result = await _productAvaliationViewModelService.DeleteProductAvaliation(id);
                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
