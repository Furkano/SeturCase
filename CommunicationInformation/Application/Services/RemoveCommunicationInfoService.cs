using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domanin.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RemoveCommunicationInfoService : IRequestHandler<RemoveCommunicationInfoRequest, bool>
    {
        private readonly ICommunicationInfoPostgresRepository<CommunicationInfo> repository;
        private readonly ILogger<RemoveCommunicationInfoService> logger;

        public RemoveCommunicationInfoService(
            ICommunicationInfoPostgresRepository<CommunicationInfo> _repository,
            ILogger<RemoveCommunicationInfoService> _logger
        )
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }
        public async Task<bool> Handle(RemoveCommunicationInfoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var info = await repository.Where(p=>p.Id==request.Id).FirstOrDefaultAsync();

                if(info!=null){
                    var result = await repository.Delete(info);
                    if(result){
                        return true;
                    }
                    else
                    {
                        logger.LogWarning("An error occurred while Deleting Communication Info.");
                        return false;
                    }
                }
                else
                { 
                    logger.LogWarning("There is no data with this id:",request.Id);
                    return false;
                }
            }
            catch(Exception ex)
            {
                logger.LogWarning(ex.Message);
                return false;
            }
        }
    }
}