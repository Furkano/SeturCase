using Domain.Entity;
using MediatR;

namespace Application.Requests
{
    public class AddPersonCallGuideRequest : IRequest<CallGuide>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }
        public int UserId { get; set; }
    }
}