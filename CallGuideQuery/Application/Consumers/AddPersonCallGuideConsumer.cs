using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Requests;
using Domain.Entity;
using Mapster;
using MassTransit;
using Microsoft.Extensions.Logging;
using Setur.Events;

namespace Application.Consumers
{
    public class AddPersonCallGuideConsumer : IConsumer<AddPersonCallGuideEvent>
    {
        private readonly ILogger<AddPersonCallGuideConsumer> logger;
        private readonly IMongoRepository repository;

        public AddPersonCallGuideConsumer
        (
            IMongoRepository _repository,
            ILogger<AddPersonCallGuideConsumer> _logger
        )
        {
            repository = _repository  ?? throw new ArgumentNullException(nameof(_repository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public async Task Consume(ConsumeContext<AddPersonCallGuideEvent> context)
        {
            Console.WriteLine("person geldi");
            try
            {
                var callGuide  = new CallGuide
                {
                    Id = context.Message.Id,
                    Firstname = context.Message.Firstname,
                    Lastname = context.Message.Lastname,
                    Company = context.Message.Company,
                    UserId = context.Message.UserId
                };
                var result  = await repository.CreateAsync(callGuide);
                if(result)
                {
                    logger.LogInformation("New person successfully created.");
                }
                else
                {
                    logger.LogWarning($"An Error while -{context.Message.Firstname}- named person inserting database!!!");
                }
                
            }
            catch(Exception excaption)
            {
                logger.LogError(excaption.Message);
            }
            
        }

    }
}