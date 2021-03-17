using System.Collections.Generic;
using Domain.Entity;
using MediatR;

namespace Application.Requests
{
    public class GetDetailAllCallGuidehUserIdRequest : IRequest<IEnumerable<CallGuide>>
    {
        public int UserId { get; set; }
    }
}