using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaportApi.Entity;
using RaportApi.Requests;

namespace RaportApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RaportController : ControllerBase
    {
        private readonly IMediator mediator;

        public RaportController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        [HttpPost]
        public async Task<Raport> CreateRaportAsync(CreateRaportRequest request)
        {
           return await mediator.Send(request);
        }
    }
}