using System;

namespace RaportApi.Entity
{
    public class Raport
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}