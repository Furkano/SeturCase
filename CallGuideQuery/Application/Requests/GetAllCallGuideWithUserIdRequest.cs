using System.Collections.Generic;
using MediatR;

namespace Application.Requests
{
    public class GetAllCallGuideWithUserIdRequest : IRequest<List<string>>
    {
        public int UserId { get; set; }
    }
}