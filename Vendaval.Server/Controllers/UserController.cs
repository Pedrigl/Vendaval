using Microsoft.AspNetCore.Mvc;
using Vendaval.Application.Services;
using Vendaval.Application.ViewModels;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Vendaval.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserViewModelSerivce _userViewModelService;

        public UserController(IUserViewModelSerivce userViewModelService)
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
                return BadRequest(ex);
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

        [Authorize]
        [HttpPut("put")]
        public async Task<IActionResult> PutAccount([FromBody] UserViewModel userViewModel)
        {
            try
            {
                var result = await _userViewModelService.PutUser(userViewModel);

                if(!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("patch")]
        public async Task<IActionResult> PatchAccount([FromBody] UserViewModel userViewModel)
        {
            try
            {
                var result = await _userViewModelService.PatchUser(userViewModel);

                if(!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteAccount([FromBody] UserViewModel userViewModel)
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
