using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entity
{
    public class Raport
    {
        [BsonId]
        public string Id { get; set; }
        
        public int UserId { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public BsonDateTime CreateDate { get; set; }
        public List<RaportItem> RaportItems {get;set;}

        public Raport()
        {
            RaportItems = new List<RaportItem>();
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}