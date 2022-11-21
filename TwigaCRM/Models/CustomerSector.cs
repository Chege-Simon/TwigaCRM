namespace TwigaCRM.Models
{
    public class CustomerSector: TimeStamps
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int SectorId { get; set; }
        public Sector Sector { get; set; }
    }
}
