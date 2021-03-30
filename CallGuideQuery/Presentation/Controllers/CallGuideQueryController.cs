using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Requests;
using Application.Resposes;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class CallGuideQueryController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMongoRepository repository;
        public CallGuideQueryController(IMediator _mediator,IMongoRepository _repository)
        {
            mediator = _mediator ?? throw new ArgumentNullException(nameof(_mediator));
            repository = _repository;
        }

        [HttpGet("GetAllCallGuideWithUserIdService/{userId}")]
        public async Task<List<string>> GetAllCallGuideWithUserIdService(int userId)
        {
            return await mediator.Send(new GetAllCallGuideWithUserIdRequest(userId));
        }
        [HttpGet("GetDetailAllCallGuidehUserIdService/{userId}")]
        public async Task<IEnumerable<CallGuide>> GetDetailAllCallGuidehUserIdService(int userId )
        {
            return await mediator.Send(new GetDetailAllCallGuidehUserIdRequest(userId));
        }
        // [HttpPost]
        // public async Task<bool> Create(CreateCallGuideRequest request)
        // {
        //     var calguide = new CallGuide
        //     {
        //         Id = request.Id,
        //         Firstname = request.Firstname,
        //         Lastname = request.Lastname,
        //         Company = request.Company,
        //         UserId = request.UserId
        //     };
        //     return await repository.CreateAsync(calguide);
        // }
        // [HttpPost("Info")]
        // public async Task<bool> CreateInfo(AddComInfoRequest request)
        // {
        //     var calguide = await repository.FindById(request.GuideId);
        //     CommunicationInfo communication = new CommunicationInfo();
        //     communication.Id = request.Id;
        //     communication.Email = request.Email;
        //     communication.Phone = request.Phone;
        //     communication.Location = request.Location;

        //     calguide.CommunicationInfos.Add(communication);
        //     await repository.UpdateAsync(calguide);
        //     return true;
        // }
        // [HttpGet("GetRaportDeneme/{userId}")]
        // public  List<RaportItem> GetRaport(int userId)
        // {
        //     var result = repository.GetRaportItem(userId);
        //     return result; 
        // }

        // Returning user call guide Locations and in that location how many people registered. 
        [HttpGet("GetRaport/{userId}")]
        public async Task<Raport> GetRaportAsync(int userId)
        {
            var request = new GetRaportWithUserId();
            request.UserId=userId;
            return await mediator.Send(request);
        }
    }
}