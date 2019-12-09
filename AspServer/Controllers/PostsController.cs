using Application.PostApp;
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
    public class PostsController : Base
    {
        [AllowAnonymous]
        [HttpGet("threadId/{threadId}")]
        public async Task<ActionResult<List<GetDTO>>> GetPosts(int threadId)
        {
            return await Mediator.Send(new GetAll.Query { ThreadId = threadId });
        }

        [AllowAnonymous]
        [HttpGet("{postId}")]
        public async Task<ActionResult<GetDTO>> GetPost(int postId)
        {
            return await Mediator.Send(new Get.Query { PostId = postId });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CreatePost(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{postId}")]
        public async Task<ActionResult<Unit>> DeletePost(int postId)
        {
            return await Mediator.Send(new Delete.Command { PostId = postId });
        }

        [HttpPut("{postId}")]
        public async Task<ActionResult<Unit>> UpdatePost(Update.Command command, int postId)
        {
            command.PostId = postId;
            return await Mediator.Send(command);
        }
    }
}