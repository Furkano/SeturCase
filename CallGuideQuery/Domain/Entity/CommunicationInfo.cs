namespace Domain.Entity
{
    public class CommunicationInfo : BaseEntity
    {
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        // public int  GuideId { get; set; }
        public CommunicationInfo(CommunicationInfo model)
        {
            Id = model.Id;
            Phone = model.Phone;
            Location= model.Location;
            Email = model.Email;
        }
        public CommunicationInfo()
        {
            
        }
    }
}