using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domanin.Interfaces;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Setur.Events;

namespace Application.Services
{
    public class AddCommunicationInfoService : IRequestHandler<AddCommunicationInfoRequest, CommunicationInfo>
    {
        private readonly ICommunicationInfoPostgresRepository<CommunicationInfo> repository;
        private readonly ILogger<AddCommunicationInfoService> logger;
        private readonly IBus bus;

        public AddCommunicationInfoService(ICommunicationInfoPostgresRepository<CommunicationInfo> _repository,
        ILogger<AddCommunicationInfoService> _logger,
        IBus _bus)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
            bus = _bus ?? throw new ArgumentNullException(nameof(_bus));
        } 
        public async Task<CommunicationInfo> Handle(AddCommunicationInfoRequest request, CancellationToken cancellationToken)
        {
            
            try
            {
                var comInfo = new CommunicationInfo
                {
                    Phone = request.Phone,
                    Location = request.Location,
                    Email = request.Email,
                    GuideId = request.GuideId,
                    UserId = request.UserId
                };
                var result = await repository.Create(comInfo);
                if(result!=null){
                    await bus.Publish(result.Adapt<AddCommunicationInfoEvent>(),cancellationToken);
                    Console.WriteLine("info publish edildi.");
                    return result;
                }
                else
                { 
                    logger.LogWarning("An error occurred while Creating Communication Info.");
                    return null;
                }
            }
            catch(Exception ex)
            {
                logger.LogWarning(ex.Message);
                return null;
            }
        }
    }
}