using System.Collections.Generic;
using Domain.Entity;

namespace Application.Requests
{
    public class CreateCallGuideRequest
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }
        public int UserId { get; set; }
        public List<CommunicationInfo> CommunicationInfos { get; set; }
    }
}