namespace Domain.Entity
{
    public class CommunicationInfo : BaseEntity
    {
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public int GuideId { get; set; }
        public int UserId { get; set; }
    }
}