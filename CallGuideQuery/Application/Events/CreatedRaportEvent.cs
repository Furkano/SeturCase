namespace Setur.Events
{
    public class CreatedRaportEvent
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int UserId { get; set;}         
    }
}