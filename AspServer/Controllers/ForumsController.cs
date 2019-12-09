using Application.ForumApp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspServer.Controllers
{
    [Authorize("AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class ForumsController : Base
    {
        [AllowAnonymous]
        [HttpGet("forumCategoryId/{id}")]
        public async Task<ActionResult<List<GetDTO>>> GetForums(int id)
        {
            return await Mediator.Send(new GetAll.Query { ForumCategoryId = id });
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetDTO>> GetForum(int id)
        {
            return await Mediator.Send(new Get.Query { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CreateForum(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> DeleteForum(int id)
        {
            return await Mediator.Send(new Delete.Command { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> UpdateForum(Update.Command command, int id)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }
    }
}