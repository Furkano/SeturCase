using System;
using MediatR;

namespace Application.Requests
{
    public class RemovePersonCallGuideRequest : IRequest<Boolean>
    {
        public int Id { get; set; }

    }
}