using System;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domain.Interfaces;
using Mapster;
using MassTransit;
using Microsoft.Extensions.Logging;
using Setur.Events;

namespace Application.Consumers
{
    public class RemovePersonCallGuideConsumer : IConsumer<RemovePersonCallGuideEvent>
    {

        private readonly ILogger<RemovePersonCallGuideConsumer> logger;
        private readonly ICallGuideMongoDbRepository<CallGuide> callGuideRepository;
        public RemovePersonCallGuideConsumer
        (
            ICallGuideMongoDbRepository<CallGuide> _callGuideRepository,
            ILogger<RemovePersonCallGuideConsumer> _logger
        )
        {
            callGuideRepository = _callGuideRepository ?? throw new ArgumentNullException(nameof(_callGuideRepository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }
        public async Task Consume(ConsumeContext<RemovePersonCallGuideEvent> context)
        {
            var message = context.Message.Adapt<DeleteCallGuideRequest>();
            try
            {
                var result = await callGuideRepository.DeleteAsync(message.Id);
                if (result)
                {
                    logger.LogInformation("CallGuide successfully Deleted.");
                }
                else
                {
                    logger.LogWarning($"An Error CallGuide ID: -{message.Id}- deleting database!!!");
                }

            }
            catch (Exception excaption)
            {
                logger.LogError(excaption.Message);
            }
        }
    }
}