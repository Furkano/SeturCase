using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class GetDetailAllCallGuidehUserIdService : IRequestHandler<GetDetailAllCallGuidehUserIdRequest, IEnumerable<CallGuide>>
    {
        private readonly ICallGuideMongoDbRepository<CallGuide> callGuideRepository;
        private readonly ILogger<GetDetailAllCallGuidehUserIdService> logger;
        
        public GetDetailAllCallGuidehUserIdService
        (
            ICallGuideMongoDbRepository<CallGuide> _callGuideRepository,
            ILogger<GetDetailAllCallGuidehUserIdService> _logger
        )
        {
            callGuideRepository = _callGuideRepository ?? throw new ArgumentNullException(nameof(_callGuideRepository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }
        public async Task<IEnumerable<CallGuide>> Handle(GetDetailAllCallGuidehUserIdRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<CallGuide> response= null;
            try
            {
                response = callGuideRepository.Where(p=>p.UserId==request.UserId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return await Task.FromResult(response);
        }
    }
}