using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RaportApi.Entity;
using RaportApi.Interfaces;
using Setur.Events;
namespace RaportApi.Consumers
{
    public class UpdateRaportConsumer : IConsumer<UpdatedRaportEvent>
    {
        private readonly IPostgreSqlRaportRepository<Raport> raportRepository;
        private readonly ILogger<UpdateRaportConsumer> logger;
        public UpdateRaportConsumer
        (
            IPostgreSqlRaportRepository<Raport> _raportRepository,
            ILogger<UpdateRaportConsumer> _logger              
        )
        {
            raportRepository = _raportRepository ?? throw new ArgumentNullException(nameof(_raportRepository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }
        public async Task Consume(ConsumeContext<UpdatedRaportEvent> context)
        {
            logger.LogInformation("Update Raport geldi.");
            try
            {
                var raport = await raportRepository.Where(p=>p.Id==context.Message.Id).FirstOrDefaultAsync();
                raport.Status = context.Message.Status;
                var result = await raportRepository.Update(raport);
                if(result!=null)
                {
                    logger.LogInformation("Raport Status is updated");
                }
                else
                {
                    logger.LogError($"An Error occured while Raport updated.Id:{context.Message.Id}");
                }
            }
            catch(Exception exception)
            {
                logger.LogError(exception.Message);
            }
        }
    }
}