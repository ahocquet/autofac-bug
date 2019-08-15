using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRExample.Api.SignalR;

namespace SignalRExample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        private readonly IHubContext<ChatHub, IChatHub> _hub;

        public SignalRController(IHubContext<ChatHub, IChatHub> hub)
        {
            _hub = hub;
        }

        [HttpGet]
        [Route("say-hello")]
        public IActionResult SayHello()
        {
            _hub.Clients.All.SayHello();

            return Ok();
        }
    }
}