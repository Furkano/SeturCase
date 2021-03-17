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
    public class AddCommunicationInfoConsumer : IConsumer<AddCommunicationInfoEvent>
    {
        private readonly ILogger<AddCommunicationInfoConsumer> logger;
        private readonly ICallGuideMongoDbRepository<CallGuide> callGuideRepository;

        public AddCommunicationInfoConsumer
        (
            ICallGuideMongoDbRepository<CallGuide> _callGuideRepository,
            ILogger<AddCommunicationInfoConsumer> _logger
        )
        {
            callGuideRepository = _callGuideRepository ?? throw new ArgumentNullException(nameof(_callGuideRepository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }
        public async Task Consume(ConsumeContext<AddCommunicationInfoEvent> context)
        {
            var message = context.Message.Adapt<AddComInfoRequest>();
            try
            {
                var callGuide = await callGuideRepository.FindById(message.GuideId);

                if (callGuide != null)
                {
                    var comInfo = new CommunicationInfo
                    {
                        Id = message.Id,
                        Phone = message.Phone,
                        Email = message.Email,
                        Location = message.Location
                    };
                    callGuide.CommunicationInfos.Add(comInfo);
                    var result = await callGuideRepository.UpdateAsync(callGuide);
                    if (result)
                    {
                        logger.LogInformation("New Comminicaion Info successfully added.");
                    }
                    else
                    {
                        logger.LogWarning($"An Error Communication InfoID: -{message.Id}- updating database!!!");
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