namespace TwigaCRM.Models
{
    public class DailyMovement: TimeStamps
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public int MainDistributorId { get; set; }
        public Customer MainDistributor { get; set; }
        public int StockistOrFarmerId { get; set; }
        public int DailyMovementReportId { get; set; }
        public DailyMovementReport DailyMovementReport { get; set; }
    }
}
