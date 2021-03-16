using Domain.Entity;

namespace Application.Requests
{
    public class AddComInfoRequest
    {
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public int GuideId { get; set; }
    }
}