namespace Application.Requests
{
    public class RemoveComInfoRequest 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GuideId { get; set; }
    }
}