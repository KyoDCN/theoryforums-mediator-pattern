using Application.ThreadApp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspServer.Controllers
{
    [Authorize("MemberOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadsController : Base
    {
        [AllowAnonymous]
        [HttpGet("forumId/{forumId}")]
        public async Task<ActionResult<List<GetAllDTO>>> GetThreads(int forumId)
        {
            return await Mediator.Send(new GetAll.Query { ForumId = forumId });
        }

        [AllowAnonymous]
        [HttpGet("{threadId}")]
        public async Task<ActionResult<GetDTO>> GetThread(int threadId)
        {
            return await Mediator.Send(new Get.Query { Id = threadId });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CreateThread(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{threadId}")]
        public async Task<ActionResult<Unit>> DeleteThread(int threadId)
        {
            return await Mediator.Send(new Delete.Command { Id = threadId });
        }

        [HttpPut("{threadId}")]
        public async Task<ActionResult<Unit>> UpdateThread(Update.Command command, int threadId)
        {
            command.Id = threadId;
            return await Mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost("{threadId}/view")]
        public async Task<ActionResult<Unit>> IncrementView(int threadId)
        {
            return await Mediator.Send(new IncrementView.Command { ThreadId = threadId });
        }
    }
}