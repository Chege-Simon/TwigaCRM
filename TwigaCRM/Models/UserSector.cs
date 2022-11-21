namespace TwigaCRM.Models
{
    public class UserSector : TimeStamps
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int SectorId { get; set; }
        public Sector Sector { get; set; }
    }
}
