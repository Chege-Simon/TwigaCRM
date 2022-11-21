namespace TwigaCRM.Models
{
    public class RAMDailyCollection : TimeStamps
    {
        public int Id { get; set; }
        public int MainDistributorId { get; set; }
        public Customer MainDistributor { get; set; }
        public decimal DailyAmount { get; set; }
        public int RAMDailyCollectionReportId { get; set; }
        public RAMDailyCollectionReport RAMDailyCollectionReport { get; set; }
    }
}
