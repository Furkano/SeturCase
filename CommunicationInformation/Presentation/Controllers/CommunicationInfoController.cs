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
    public class CommunicationInfoController : Controller
    {
        private readonly IMediator mediator;
        public CommunicationInfoController(IMediator _mediator)
        {
            mediator = _mediator ?? throw new ArgumentNullException(nameof(_mediator));
        }
        [HttpPost]
        public async Task<CommunicationInfo> AddCommunicationInfo(AddCommunicationInfoRequest addCommunicationInfoRequest)
        {
            return await mediator.Send(addCommunicationInfoRequest);
        }
        [HttpDelete]
        public async Task<Boolean> RemoveCommunicationInfo(RemoveCommunicationInfoRequest removeCommunicationInfoRequest)
        {
            return await mediator.Send(removeCommunicationInfoRequest);
        }
    }
}