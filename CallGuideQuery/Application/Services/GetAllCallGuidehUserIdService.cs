using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Data.Entity;

namespace Application.Services
{
    public class GetAllCallGuideWithUserIdService : IRequestHandler<GetAllCallGuideWithUserIdRequest, List<string>>
    {
        private readonly ICallGuideMongoDbRepository<CallGuide> callGuideRepository;
        private readonly ILogger<GetAllCallGuideWithUserIdService> logger;
        
        public GetAllCallGuideWithUserIdService
        (
            ICallGuideMongoDbRepository<CallGuide> _callGuideRepository,
            ILogger<GetAllCallGuideWithUserIdService> _logger
        )
        {
            callGuideRepository = _callGuideRepository ?? throw new ArgumentNullException(nameof(_callGuideRepository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }
        public async Task<List<string>> Handle(GetAllCallGuideWithUserIdRequest request, CancellationToken cancellationToken)
        {

            List<string> response = new List<string>();
            try
            {
                var data = callGuideRepository.Where(p=>p.UserId==request.UserId).ToList();
                data.ForEach(f=>{
                    response.Add(f.Firstname+"  "+f.Lastname);
                });

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return await Task.FromResult(response);
        }
    }
}