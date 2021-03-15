using MediatR;

namespace Application.Requests
{
    public class RemoveCommunicationInfoRequest : IRequest<bool>
    {
        public int Id { get; set; }
    }
}