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
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
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
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _userViewModelService.GetUserAsync(id);

                if(!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userViewModelService.GetAllUsersAsync();

                if(!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var result = await _userViewModelService.DeleteLogin(id);

                if(!result.Success)
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
