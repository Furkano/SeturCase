using System;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallGuideCommandController : Controller
    {
        private readonly IMediator mediator;
        public CallGuideCommandController(IMediator _mediator)
        {
            mediator = _mediator ?? throw new ArgumentNullException(nameof(_mediator));
        }
        [HttpPost]
        public async Task<CallGuide> AddPersonCallGuide(AddPersonCallGuideRequest addPersonCallGuideRequest)
        {
            return await mediator.Send(addPersonCallGuideRequest);
        }
        [HttpDelete]
        public async Task<Boolean> RemovePersonCallGuide(RemovePersonCallGuideRequest removePersonCallGuideRequest)
        {
            return await mediator.Send(removePersonCallGuideRequest);
        }
    }
}