using Domain.Entity;
using MediatR;

namespace Application.Requests
{
    public class GetRaportWithUserId : IRequest<Raport>
    {
        public int UserId { get; set; }
        
        
    }
}