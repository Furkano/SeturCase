namespace Domain.Entity
{
    public class CallGuide : BaseEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }
        public int UserId { get; set; }
    }
}