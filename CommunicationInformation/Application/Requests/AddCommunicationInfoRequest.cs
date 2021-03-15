using Domain.Entity;
using MediatR;

namespace Application.Requests
{
    public class AddCommunicationInfoRequest : IRequest<CommunicationInfo>
    {
        
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public int GuideId { get; set; }
        public int UserId { get; set; }
    }
}