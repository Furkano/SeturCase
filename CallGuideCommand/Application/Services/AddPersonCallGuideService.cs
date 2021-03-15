using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AddPersonCallGuideService : IRequestHandler<AddPersonCallGuideRequest, CallGuide>
    {
        private readonly ICallGuideRepository<CallGuide> callGuideRepository;
        private readonly ILogger<AddPersonCallGuideService> logger;
        public AddPersonCallGuideService(
            ICallGuideRepository<CallGuide> _callGuideRepository,
            ILogger<AddPersonCallGuideService> _logger
            )
        {
            callGuideRepository = _callGuideRepository ?? throw new ArgumentNullException(nameof(_callGuideRepository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }
        public async Task<CallGuide> Handle(AddPersonCallGuideRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CallGuide callGuide = new CallGuide
                {
                    Firstname = request.Firstname,
                    Lastname = request.Lastname,
                    Company = request.Company,
                    UserId = request.UserId
                };
                var result = await callGuideRepository.Create(callGuide);
                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                logger.LogError(exception.Message);
                return null;
            }
        }
    }
}