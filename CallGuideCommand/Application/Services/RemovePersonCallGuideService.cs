using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domain.Interfaces;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Setur.Events;

namespace Application.Services
{
    public class RemovePersonCallGuideService : IRequestHandler<RemovePersonCallGuideRequest, Boolean>
    {
        private readonly ICallGuideRepository<CallGuide> callGuideRepository;
        private readonly ILogger<RemovePersonCallGuideService> logger;
        private readonly IBus bus;
        public RemovePersonCallGuideService
            (
                ICallGuideRepository<CallGuide> _callGuideRepository,
                ILogger<RemovePersonCallGuideService> _logger,
                IBus _bus
            )
        {
            callGuideRepository = _callGuideRepository ?? throw new ArgumentNullException(nameof(_callGuideRepository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
            bus = _bus ?? throw new ArgumentNullException(nameof(_bus));
        }
        public async Task<bool> Handle(RemovePersonCallGuideRequest request, CancellationToken cancellationToken)
        {
            var result = false;
            try
            {
                var data = await callGuideRepository.Where(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
                if (data != null)
                {
                    result = await callGuideRepository.Delete(data);
                    if(result)
                    {
                        await bus.Publish(data.Adapt<RemovePersonCallGuideEvent>(),cancellationToken);
                    }
                    else
                    {
                        logger.LogError("There is an error occured while deleting data");
                    }
                }else{
                    logger.LogError($"There no data that CallGuideID:{request.Id}");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                logger.LogError(exception.Message);
            }
            return result;
        }
    }
}