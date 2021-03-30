#nullable enable
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entity
{
    public class CallGuide : BaseEntity
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Company { get; set; } = null!;
        public int UserId { get; set; }
        
        public List<CommunicationInfo>? CommunicationInfos { get; set; }
        
        
        public CallGuide()
        {
            CommunicationInfos = new List<CommunicationInfo>();
        }
    }
}