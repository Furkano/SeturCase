using System;
using MediatR;
using RaportApi.Entity;

namespace RaportApi.Requests
{
    public class CreateRaportRequest : IRequest<Raport>
    {
        public int UserId { get; set; }
    }
}