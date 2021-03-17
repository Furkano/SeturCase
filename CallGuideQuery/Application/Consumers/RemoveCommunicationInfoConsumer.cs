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
    public class RemoveCommunicationInfoConsumer : IConsumer<RemoveCommunicationInfoEvent>
    {
        private readonly ILogger<RemoveCommunicationInfoConsumer> logger;
        private readonly ICallGuideMongoDbRepository<CallGuide> callGuideRepository;
        public RemoveCommunicationInfoConsumer
        (
            ICallGuideMongoDbRepository<CallGuide> _callGuideRepository,
            ILogger<RemoveCommunicationInfoConsumer> _logger
        )
        {
            callGuideRepository = _callGuideRepository ?? throw new ArgumentNullException(nameof(_callGuideRepository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }
        public async Task Consume(ConsumeContext<RemoveCommunicationInfoEvent> context)
        {
            var message = context.Message.Adapt<RemoveComInfoRequest>();
            try
            {
                var callGuide = await callGuideRepository.FindById(message.GuideId);
                if (callGuide != null)
                {
                    var comInfo = callGuide.CommunicationInfos.Find(p => p.Id == message.Id);

                    callGuide.CommunicationInfos.Remove(comInfo);

                    var result = await callGuideRepository.UpdateAsync(callGuide);

                    if (result)
                    {
                        logger.LogInformation("Comminicaion Info successfully Removed.");
                    }
                    else
                    {
                        logger.LogWarning($"An Error Communication InfoID: -{message.Id}- deleting database!!!");
                    }
                }
                else
                {
                    logger.LogWarning($"An Error. There is no CallGuide which that ID:-{message.GuideId}!!!");
                }
            }
            catch (Exception excaption)
            {
                logger.LogError(excaption.Message);
            }
        }
    }
}