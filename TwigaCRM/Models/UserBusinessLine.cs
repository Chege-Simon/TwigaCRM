namespace TwigaCRM.Models
{
    public class UserBusinessLine : TimeStamps
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int BusinessLineId { get; set; }
        public BusinessLine BusinessLine { get; set; }
    }
}
