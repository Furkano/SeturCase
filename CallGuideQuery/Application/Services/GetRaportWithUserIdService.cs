using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Requests;
using Domain.Entity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class GetRaportWithUserIdService : IRequestHandler<GetRaportWithUserId, Raport>
    {
        private readonly IRaportRepository raportRepository;
        private readonly ILogger<GetRaportWithUserIdService> logger;
        public GetRaportWithUserIdService
        (
            IRaportRepository _raportRepository,
            ILogger<GetRaportWithUserIdService> _logger
        )
        {
            raportRepository= _raportRepository;
            logger = _logger;
        }
        public async Task<Raport> Handle(GetRaportWithUserId request, CancellationToken cancellationToken)
        {
            
            try
            {
                return await raportRepository.GetRaport(request.UserId);
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
            
        }
    }
}