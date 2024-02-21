using Microsoft.AspNetCore.Mvc;
using Vendaval.Application.Services;
using Vendaval.Application.ViewModels;

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

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel userViewModel)
        {
            var user = await _userViewModelService.Login(userViewModel);

            if (userViewModel == null)
            {
                return Unauthorized("");
            }
            return Ok(userViewModel);
        }
    }
}
