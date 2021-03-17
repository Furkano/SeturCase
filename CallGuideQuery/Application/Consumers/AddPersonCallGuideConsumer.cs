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
    public class AddPersonCallGuideConsumer : IConsumer<AddPersonCallGuideEvent>
    {
        private readonly ILogger<AddPersonCallGuideConsumer> logger;
        private readonly ICallGuideMongoDbRepository<CallGuide> repository;

        public AddPersonCallGuideConsumer
        (
            ICallGuideMongoDbRepository<CallGuide> _repository,
            ILogger<AddPersonCallGuideConsumer> _logger
        )
        {
            repository = _repository  ?? throw new ArgumentNullException(nameof(_repository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public async Task Consume(ConsumeContext<AddPersonCallGuideEvent> context)
        {
            var message = context.Message.Adapt<CreateCallGuideRequest>();
            try
            {
                var callGuide  = new CallGuide
                {
                    Id = message.Id,
                    Firstname = message.Firstname,
                    Lastname = message.Lastname,
                    Company = message.Company,
                    UserId = message.UserId
                };
                var result  = await repository.CreateAsync(callGuide);
                if(result)
                {
                    logger.LogInformation("New person successfully created.");
                }
                else
                {
                    logger.LogWarning($"An Error while -{message.Firstname}- named person inserting database!!!");
                }
                
            }
            catch(Exception excaption)
            {
                logger.LogError(excaption.Message);
            }
            
        }

    }
}