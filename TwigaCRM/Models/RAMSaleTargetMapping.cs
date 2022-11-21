namespace TwigaCRM.Models
{
    public class RAMSaleTargetMapping:TimeStamps
    {
        public int Id { get; set; }
        public int MainDistributorId { get; set; }
        public Customer MainDistributor { get; set; }
        public decimal ExpectedAmount { get; set; }
        public int RAMSaleTargetId { get; set; }
        public RAMSaleTarget RAMSaleTarget { get; set; }
    }
}
