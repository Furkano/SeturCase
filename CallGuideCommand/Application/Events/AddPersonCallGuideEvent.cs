namespace Application.Events
{
    public class AddPersonCallGuideEvent
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }
        public int UserId { get; set; }
    }
}