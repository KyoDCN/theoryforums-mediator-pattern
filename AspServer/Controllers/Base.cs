using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AspServer.Controllers
{
    /// <summary>
    /// Abstract Base controller class to implement reuseable properties
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class Base : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator { get => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>()); }
    }
}
