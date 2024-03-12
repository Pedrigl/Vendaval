using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Vendaval.Application.Services;
using Vendaval.Application.ViewModels;

namespace Vendaval.Api.Controllers
{
    // ChatController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _chatHub;

        public ChatController(IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
        }

        [HttpGet("onlineSellers")]
        public async Task<IActionResult> GetOnlineSellers()
        {
            await _chatHub.Clients.All.SendAsync("OnlineSellers");
            return Ok();
        }

        [HttpGet("onlineCostumers")]
        public async Task<IActionResult> GetOnlineCostumers()
        {
            await _chatHub.Clients.All.SendAsync("OnlineCostumers");
            return Ok();
        }

        [HttpGet("connected")]
        public async Task<IActionResult> GetConnected()
        {
            await _chatHub.Clients.All.SendAsync("Connected");
            return Ok();
        }

        [HttpGet("disconnected")]
        public async Task<IActionResult> GetDisconnected()
        {
            await _chatHub.Clients.All.SendAsync("Disconnected");
            return Ok();
        }

        [HttpPost("privateMessage")]
        public async Task<IActionResult> SendPrivateMessage(string receiverId, [FromBody] MessageViewModel message)
        {
            await _chatHub.Clients.Client(receiverId).SendAsync("ReceivePrivateMessage", message);
            return Ok();
        }
    }

}
