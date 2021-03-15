using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domanin.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AddCommunicationInfoService : IRequestHandler<AddCommunicationInfoRequest, CommunicationInfo>
    {
        private readonly ICommunicationInfoPostgresRepository<CommunicationInfo> repository;
        private readonly ILogger<AddCommunicationInfoService> logger;

        public AddCommunicationInfoService(ICommunicationInfoPostgresRepository<CommunicationInfo> _repository,
        ILogger<AddCommunicationInfoService> _logger)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        } 
        public async Task<CommunicationInfo> Handle(AddCommunicationInfoRequest request, CancellationToken cancellationToken)
        {
            
            try
            {
                var comInfo = new CommunicationInfo
                {
                    Phone = request.Phone,
                    Location = request.Location,
                    Email = request.Email,
                    GuideId = request.GuideId,
                    UserId = request.UserId
                };
                var result = await repository.Create(comInfo);
                if(result!=null){
                    return result;
                }
                else
                { 
                    logger.LogWarning("An error occurred while Creating Communication Info.");
                    return null;
                }
            }
            catch(Exception ex)
            {
                logger.LogWarning(ex.Message);
                return null;
            }
        }
    }
}