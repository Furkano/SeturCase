using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Presentation.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class CallGuideQueryController : Controller
    {
        private readonly IMediator mediator;
        public CallGuideQueryController(IMediator _mediator)
        {
            mediator = _mediator ?? throw new ArgumentNullException(nameof(_mediator));
        }

        [HttpGet("GetAllCallGuideWithUserIdService")]
        public async Task<List<string>> GetAllCallGuideWithUserIdService(GetAllCallGuideWithUserIdRequest request)
        {
            return await mediator.Send(request);
        }
        [HttpGet("GetDetailAllCallGuidehUserIdService")]
        public async Task<IEnumerable<CallGuide>> GetDetailAllCallGuidehUserIdService(GetDetailAllCallGuidehUserIdRequest request)
        {
            return await mediator.Send(request);
        }

    }
}