using System;
using System.Threading.Tasks;
using Application.Events;
using Application.Interfaces;
using Domain.Entity;
using MassTransit;
using Microsoft.Extensions.Logging;
using Setur.Events;

namespace Application.Consumers
{
    public class CreatedRaportConsumer : IConsumer<CreatedRaportEvent>
    {
        private readonly ILogger<CreatedRaportConsumer> logger;
        private readonly IMongoRepository callGuideRepository;
        private readonly IRaportRepository raportRepository;
        private readonly IBus bus;
        public  CreatedRaportConsumer
        (
            IMongoRepository _callGuideRepository,
            IRaportRepository _raportRepository,
            ILogger<CreatedRaportConsumer> _logger,
            IBus _bus  
        )
        {
            callGuideRepository = _callGuideRepository;
            raportRepository= _raportRepository;
            logger = _logger;
            bus = _bus;
        }
        public async Task Consume(ConsumeContext<CreatedRaportEvent> context)
        {
            logger.LogInformation("Created Raport geldi.");
            try
            {
                var raportItemList = callGuideRepository.GetRaportItem(context.Message.UserId);
                Raport raport = new Raport();
                raport.UserId = context.Message.UserId;
                raport.CreateDate = DateTime.UtcNow;
                raport.RaportItems.AddRange(raportItemList);
                await raportRepository.CreateAsync(raport);
                var updateEvent = new UpdatedRaportEvent(){
                    Id = context.Message.Id,
                    Status = "TAMAMLANDI"
                };
                await bus.Publish(updateEvent);
            }
            catch(Exception exception)
            {
                logger.LogError(exception.Message);
            }
        }
    }
}