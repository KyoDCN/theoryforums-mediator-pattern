using Application.ForumCategoryApp;
using AspServer.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspServer.Controllers
{
    [Route("api/[controller]")]
    [Authorize("AdminOnly")]
    [ApiController]
    public class ForumCategoriesController : Base
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<GetAllDTO>>> GetForumCategories()
        {
            return await Mediator.Send(new GetAll.Query());
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CreateForumCategory(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{forumCategoryId}")]
        public async Task<ActionResult<Unit>> DeleteForumCategory(int forumCategoryId)
        {
            return await Mediator.Send(new Delete.Command { Id = forumCategoryId });
        }

        [HttpPut("{forumCategoryId}")]
        public async Task<ActionResult<Unit>> UpdateForumCategory(Update.Command command, int forumCategoryId)
        {
            command.Id = forumCategoryId;
            return await Mediator.Send(command);
        }
    }
}