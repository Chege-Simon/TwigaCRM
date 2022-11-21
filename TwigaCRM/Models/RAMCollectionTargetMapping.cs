namespace TwigaCRM.Models
{
    public class RAMCollectionTargetMapping:TimeStamps
    {
        public int Id { get; set; }
        public int MainDistributorId { get; set; }
        public Customer MainDistributor { get; set; }
        public decimal ExpectedCollection { get; set; }
        public int RAMCollectionTargetId { get; set; }
        public RAMCollectionTarget RAMCollectionTarget { get; set; }
    }
}
