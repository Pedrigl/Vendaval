using Microsoft.AspNetCore.Mvc;
using Vendaval.Application.Services;
using Vendaval.Application.ViewModels;
using Vendaval.Application.ValueObjects;

namespace Vendaval.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserViewModelService _userViewModelService;

        public UserController(UserViewModelService userViewModelService)
        {
            _userViewModelService = userViewModelService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto userViewModel)
        {
            try
            {
                var result = await _userViewModelService.Login(userViewModel);

                if(!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserViewModel userViewModel)
        {
            try
            {
                var result = await _userViewModelService.Register(userViewModel);

                if(!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserViewModel userViewModel)
        {
            try
            {
                var result = await _userViewModelService.UpdateLogin(userViewModel);

                if(!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromBody] UserViewModel userViewModel)
        {
            try
            {
                var result = _userViewModelService.DeleteLogin(userViewModel);

                if(!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
