using System;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using RaportApi.Entity;
using RaportApi.Interfaces;
using RaportApi.Requests;
using Setur.Events;

namespace RaportApi.Services
{
    public class CreateRaportService : IRequestHandler<CreateRaportRequest, Raport>
    {
        private readonly IPostgreSqlRaportRepository<Raport> raportRepository;
        private readonly ILogger<CreateRaportService> logger;
        private readonly IBus bus;

        public CreateRaportService
        (
            IPostgreSqlRaportRepository<Raport> _raportRepository,
            ILogger<CreateRaportService> _logger,
            IBus _bus    
        )
        {
            raportRepository = _raportRepository ?? throw new ArgumentNullException(nameof(_raportRepository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
            bus = _bus ?? throw new ArgumentNullException(nameof(_bus));
        }
        public async Task<Raport> Handle(CreateRaportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var raport = new Raport
                {
                    CreateDate = DateTime.Now,
                    Status = "HAZIRLANIYOR",
                    ModifiedDate = DateTime.Now,
                    UserId = request.UserId
                };
                var result = await raportRepository.Create(raport);
                await bus.Publish(result.Adapt<CreatedRaportEvent>(),cancellationToken);
                return result;
            }
            catch(Exception exception)
            {
                logger.LogError(exception.Message);
                return null;
            }
        }
    }
}