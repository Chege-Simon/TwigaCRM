namespace TwigaCRM.Models
{
    public class RAMDailySale:TimeStamps
    {
        public int Id { get; set; }
        public int MainDistributorId { get; set; }
        public Customer MainDistributor { get; set; }
        public decimal DailyAmount { get; set; }
        public int RAMDailySaleReportId { get; set; }
        public RAMDailySaleReport RAMDailySaleReport { get; set; }
    }
}
