namespace Setur.Events
{
    public class AddCommunicationInfoEvent
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public int GuideId { get; set; }
        public int UserId { get; set; }        
    }
}