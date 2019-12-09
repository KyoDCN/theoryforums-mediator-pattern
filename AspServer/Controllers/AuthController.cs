using Application.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspServer.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Base
    {
        [HttpPost("register")]
        public async Task<ActionResult<RegisterDTO>> Register(Register.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginDTO>> Login(Login.Query query)
        {
            return await Mediator.Send(query);
        }
    }
}