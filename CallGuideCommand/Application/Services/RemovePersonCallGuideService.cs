using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RemovePersonCallGuideService : IRequestHandler<RemovePersonCallGuideRequest, Boolean>
    {
        private readonly ICallGuideRepository<CallGuide> callGuideRepository;
        private readonly ILogger<RemovePersonCallGuideService> logger;
        public RemovePersonCallGuideService
            (
                ICallGuideRepository<CallGuide> _callGuideRepository,
                ILogger<RemovePersonCallGuideService> _logger
            )
        {
            callGuideRepository = _callGuideRepository ?? throw new ArgumentNullException(nameof(_callGuideRepository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
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