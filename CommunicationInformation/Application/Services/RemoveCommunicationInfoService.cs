using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domanin.Interfaces;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Setur.Events;

namespace Application.Services
{
    public class RemoveCommunicationInfoService : IRequestHandler<RemoveCommunicationInfoRequest, bool>
    {
        private readonly ICommunicationInfoPostgresRepository<CommunicationInfo> repository;
        private readonly ILogger<RemoveCommunicationInfoService> logger;
        private readonly IBus bus;

        public RemoveCommunicationInfoService(
            ICommunicationInfoPostgresRepository<CommunicationInfo> _repository,
            ILogger<RemoveCommunicationInfoService> _logger,
            IBus _bus
        )
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
            bus = _bus ?? throw new ArgumentNullException(nameof(_bus));
        }
        public async Task<bool> Handle(RemoveCommunicationInfoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var info = await repository.Where(p=>p.Id==request.Id).FirstOrDefaultAsync();

                if(info!=null){
                    var result = await repository.Delete(info);
                    if(result){
                        await bus.Publish(info.Adapt<RemoveCommunicationInfoEvent>(),cancellationToken);
                        return true;
                    }
                    else
                    {
                        logger.LogWarning("An error occurred while Deleting Communication Info.");
                        return false;
                    }
                }
                else
                { 
                    logger.LogWarning("There is no data with this id:",request.Id);
                    return false;
                }
            }
            catch(Exception ex)
            {
                logger.LogWarning(ex.Message);
                return false;
            }
        }
    }
}